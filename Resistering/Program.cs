using Consul;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register Consul client
builder.Services.AddSingleton<IConsulClient, ConsulClient>(c => new ConsulClient(config =>
{
    // Assume Consul agent is running locally on port 8500
    config.Address = new Uri("http://localhost:8500");
}));
   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register the service with Consul
var consulClient = app.Services.GetRequiredService<IConsulClient>();
RegisterServiceWithConsul(consulClient, app.Lifetime);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



void RegisterServiceWithConsul(IConsulClient consulClient, IHostApplicationLifetime lifetime)
{
    var registration = new AgentServiceRegistration
    {
        ID = Guid.NewGuid().ToString(), // Unique service ID
        Name = "MyMicroservice",        // Service name in Consul
        Address = Dns.GetHostName(),    // The address of the service
        Port = 7286,
        Tags = new[] { "https" },  // Optional tags 
        // The port the service is running on
        Check = new AgentServiceCheck
        {
            HTTP = "https://localhost:7286/health", // Health check endpoint
            Interval = TimeSpan.FromSeconds(10),   // Interval between health checks
            Timeout = TimeSpan.FromSeconds(5)      // Timeout for health check
        }
    };

    consulClient.Agent.ServiceRegister(registration).Wait();

    // Unregister the service on application shutdown
    lifetime.ApplicationStopping.Register(() =>
    {
        consulClient.Agent.ServiceDeregister(registration.ID).Wait();
    });
}
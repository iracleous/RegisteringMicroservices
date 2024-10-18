How to install consul on docker desktop


docker pull hashicorp/consul

docker run -d --name=consul -p 8500:8500 -p 8600:8600/udp hashicorp/consul

http://localhost:8500
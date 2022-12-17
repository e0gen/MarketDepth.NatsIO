# MarketDepth.NatsIO

Pub/Sub service taking depth quotes from Binance and expose them trough websocket to frontend with Nats.IO streaming

This is an example .Net project for running local development environment. It features:

* Running multiple Docker containers with [Docker Compose](https://docs.docker.com/compose)
* [Publisher](publisher/main.go) and [Subscriber](subscriber/main.go) services
* Communication over [NATS](https://nats.io/) between two services

## Running

To start the services, simply run:

```
docker-compose up
```
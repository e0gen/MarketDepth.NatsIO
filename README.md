# MarketDepth.NatsIO

Pub/Sub service taking depth quotes from Binance and expose them trough websocket to frontend with Nats.IO streaming

This is an example .Net project for running local development environment. It features:

* Running multiple Docker containers with [Docker Compose](https://docs.docker.com/compose)
* [Publisher](src/MarketDepth.Pub) and [Subscriber](src/MarketDepth.Sub.WsServer) services
* Communication over [NATS](https://nats.io/) between two services
* Subscriber get data and expose it to the WebSocket channel `ws://localhost:5050/public.depth.${symbol}`

## Setting up

Put your Binance API keys into .env file

```
BINANCE_API_KEY=YOUR-API-KEY
BINANCE_SECRET_KEY=YOUR-SECRET-KEY
```
Change interested symbol
```
SYMBOL=btcusdt 
```

## Running

To start the services, simply run:

```
docker-compose up
```

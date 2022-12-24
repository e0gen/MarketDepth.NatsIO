# MarketDepth.NatsIO

Pub/Sub service taking depth quotes from Binance and expose them trough websocket to frontend with Nats.IO streaming

This is a demo .Net project for running in local development environment. It features:

* Running multiple Docker containers with [Docker Compose](https://docs.docker.com/compose)
* [Publisher](src/MarketDepth.Pub) and [Subscriber](src/MarketDepth.Sub.WsServer) services
* Communication over [NATS](https://nats.io/) between two services
* [Publisher](src/MarketDepth.Pub) connects to Binance websocket API with [BinanceNet](https://github.com/JKorf/Binance.Net)
* [Subscriber](src/MarketDepth.Sub.WsServer) get data and expose it to the WebSocket channel `ws://localhost:5050/public.depth.${symbol}`
* [ClientUI](src/MarketDepth.ClientUI) service built with Angular 15 and served by Nginx on `https://localhost:4004/`

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
## Building

To build containers, run:

```
docker-compose build
```

## Running

To start the services, run:

```
docker-compose up
```

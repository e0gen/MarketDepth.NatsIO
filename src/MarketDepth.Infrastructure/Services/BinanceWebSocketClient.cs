using BinanceExchange.API.Websockets;
using BinanceExchange.API.Client;
using BinanceExchange.API.Client.Interfaces;
using MarketDepth.Domain.Models;
using MarketDepth.Application.Contracts;

namespace MarketDepth.Infrastructure.Services
{
    public sealed class BinanceWebSocketClient : IExchangeSocketClient
    {
        private IBinanceClient _binanceClient;
        private IBinanceWebSocketClient _binanceWebSocketClient;

        public BinanceWebSocketClient(IOptions<Config> options)
        {
            _binanceClient = new BinanceClient(new ClientConfiguration
            {
                ApiKey = options.Value.ApiKey,
                SecretKey = options.Value.SecretKey
            });
            _binanceWebSocketClient = new InstanceBinanceWebSocketClient(_binanceClient);
        }

        public Guid ConnectToDepthWebSocket(Action<DepthData> messageEventHandler)
        {
            return _binanceWebSocketClient.ConnectToDepthWebSocket("BTCUSDT", data =>
            {
                var depthData = data.Adapt<DepthData>();

                messageEventHandler(depthData);
            });
        }

        public void CloseWebSocketInstance(Guid guid)
        {
            _binanceWebSocketClient.CloseWebSocketInstance(guid);
        }

        public class Config
        {
            public string ApiKey { get; set; }
            public string SecretKey { get; set; }
        }
    }
}
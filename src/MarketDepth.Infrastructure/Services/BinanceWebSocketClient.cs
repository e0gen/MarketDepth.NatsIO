using BinanceExchange.API.Websockets;
using BinanceExchange.API.Client;
using BinanceExchange.API.Client.Interfaces;

namespace MarketDepth.Infrastructure.Services
{
    public sealed class BinanceWebSocketClient : IExchangeSocketClient
    {
        private readonly IBinanceClient _binanceClient;
        private readonly IBinanceWebSocketClient _binanceWebSocketClient;
        private readonly Settings _settings;
        public BinanceWebSocketClient(IOptions<Settings> options, IConfiguration config)
        {
            _settings = options.Value;
            _settings.ApiKey = config.GetVarFromEnvironment("BINANCE_API_KEY");
            _settings.SecretKey = config.GetVarFromEnvironment("BINANCE_SECRET_KEY");

            _binanceClient = new BinanceClient(new ClientConfiguration
            {
                ApiKey = _settings.ApiKey,
                SecretKey = _settings.SecretKey
            });
            _binanceWebSocketClient = new InstanceBinanceWebSocketClient(_binanceClient);
        }

        public Guid ConnectToDepthWebSocket(string symbol, Action<DepthData> messageEventHandler)
        {
            return _binanceWebSocketClient.ConnectToDepthWebSocket(symbol, data =>
            {
                var depthData = data.Adapt<DepthData>();

                messageEventHandler(depthData);
            });
        }

        public void CloseWebSocketInstance(Guid guid)
        {
            _binanceWebSocketClient.CloseWebSocketInstance(guid);
        }

        public class Settings
        {
            public string ApiKey { get; set; }
            public string SecretKey { get; set; }
        }
    }
}
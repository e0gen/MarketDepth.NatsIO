using BinanceExchange.API.Websockets;
using BinanceExchange.API.Client;
using BinanceExchange.API.Client.Interfaces;

namespace MarketDepth.Infrastructure.Services
{
    public sealed class BinanceWebSocketClient : IExchangeSocketClient
    {
        private readonly ILogger<BinanceWebSocketClient> _logger;
        private readonly IBinanceClient _binanceClient;
        private readonly IBinanceWebSocketClient _binanceWebSocketClient;
        private readonly Settings _settings;

        public BinanceWebSocketClient(ILogger<BinanceWebSocketClient> logger, IOptions<Settings> options, IConfiguration config)
        {
            _logger = logger;
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

        public Guid ConnectToDepthWebSocket(string symbol, Action<MarketEvent<DepthUpdate>> messageEventHandler)
        {
            return _binanceWebSocketClient.ConnectToDepthWebSocket(symbol, data =>
            {
                MarketEvent<DepthUpdate> depthData = new MarketEvent<DepthUpdate>
                {
                    EventType = data.EventType,
                    EventTime = data.EventTime,
                    Data = data.Adapt<DepthUpdate>()
                };

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
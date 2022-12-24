using MarketDepth.Application.Contracts;
using MarketDepth.Infrastructure.Utils;
using System.Text.Json;

namespace MarketDepth.Pub
{
    public sealed class DepthPublisher : BackgroundService
    {
        private readonly ILogger<DepthPublisher> _logger;
        private readonly IExchangeSocketClient _exchangeSocketClient;
        private readonly IMessageBusClient _messageBus;
        private readonly string _symbol;
        private Guid _exchangeSocketGuid;

        public DepthPublisher(ILogger<DepthPublisher> logger,
            IExchangeSocketClient exchangeSocketClient,
            IMessageBusClient messageBus,
            IConfiguration config)
        {
            _logger = logger;
            _exchangeSocketClient = exchangeSocketClient;
            _messageBus = messageBus;
            _symbol = config.GetVarFromEnvironment("SYMBOL") ??
                throw new Exception($"Environment variable required: SYMBOL");
        }
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            _exchangeSocketGuid = _exchangeSocketClient.ConnectToDepthWebSocket(_symbol, async (depthData) =>
            {
                Console.WriteLine($"[Binance Event]: {depthData.EventType} {depthData.EventTime} {depthData.Data.Symbol} {depthData.Data.UpdateId}");

                var payload = JsonSerializer.SerializeToUtf8Bytes(depthData, depthData.GetType());

                string guid = _messageBus.Publish(payload);
            });

            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(10000, ct);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _exchangeSocketClient.CloseWebSocketInstance(_exchangeSocketGuid);

            return base.StopAsync(cancellationToken);
        }
    }
}
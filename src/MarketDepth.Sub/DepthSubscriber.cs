using MarketDepth.Application.Contracts;
using MarketDepth.Infrastructure.Utils;

namespace MarketDepth.Sub
{
    public class DepthSubscriber : BackgroundService
    {
        private readonly ILogger<DepthSubscriber> _logger;
        private readonly IMessageBusClient _messageBus;
        private readonly string _symbol;

        public DepthSubscriber(ILogger<DepthSubscriber> logger,
            IMessageBusClient messageBus,
            IConfiguration config)
        {
            _logger = logger;
            _messageBus = messageBus; 
            _symbol = config.GetVarFromEnvironment("SYMBOL") ??
                throw new Exception($"Environment variable required: SYMBOL");
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await _messageBus.Subscribe((data) =>
                {
                    Console.WriteLine($"[Nats Event] Bytes recieved: {data.Length}");

                }, ct);
            }
        }
    }
}
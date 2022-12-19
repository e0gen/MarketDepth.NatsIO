using STAN.Client;

namespace MarketDepth.Infrastructure.Services
{
    public class NatsStreamingClient : IMessageBusClient
    {
        private Settings _config;
        private StanConnectionFactory _cf;
        private IStanConnection _c;

        public NatsStreamingClient(IOptions<Settings> options, 
            IConfiguration config)
        {
            _config = options.Value;
            _config.NatsUrl ??= config.GetVarFromEnvironment("NATS_URL");
            _config.Channel ??= config.GetVarFromEnvironment("NATS_CHANNEL");
            _config.ClusterId ??= config.GetVarFromEnvironment("NATS_CLUSTER_ID");

            _cf = new StanConnectionFactory();
            _c = CreateConnection();
        }

        public string Publish(byte[] payload)
        {
            if (_c.NATSConnection.IsClosed())
                _c = CreateConnection();

            return _c.Publish(_config.Channel, payload, PublishAckHandler);
        }

        public async Task<string> PublishAsync(byte[] payload)
        {
            if (_c.NATSConnection.IsClosed())
                _c = CreateConnection();

            return await _c.PublishAsync(_config.Channel, payload);
        }

        public async Task Subscribe(Action<byte[]> messageHandler, CancellationToken ct) 
        {
            if (_c.NATSConnection.IsClosed())
                _c = CreateConnection();

            var subOpts = StanSubscriptionOptions.GetDefaultOptions();
            subOpts.StartWithLastReceived();

            var s = _c.Subscribe(_config.Channel, subOpts, (obj, args) =>
            {
                Console.WriteLine("Received a message");

                messageHandler(args.Message.Data);
            });

            while(!ct.IsCancellationRequested)
            {
                await Task.Delay(1000);
            }

            s.Unsubscribe();
        }
        private IStanConnection CreateConnection()
        {
            var opts = StanOptions.GetDefaultOptions();
            opts.NatsURL = _config.NatsUrl;
            return _cf.CreateConnection(_config.ClusterId, _config.ClientId, opts);
        }
        private void PublishAckHandler(object? obj, StanAckHandlerArgs args)
        {
            if (!string.IsNullOrEmpty(args.Error))
            {
                Console.WriteLine("Published Msg {0} failed: {1}",
                    args.GUID, args.Error);
            }

            Console.WriteLine("Published msg {0} was stored on the server.", args.GUID);
        }

        public class Settings
        {
            public string ClusterId { get; set; }
            public string ClientId { get; set; }
            public string NatsUrl { get; set; }
            public string Channel { get; set; }
        }
    }
}

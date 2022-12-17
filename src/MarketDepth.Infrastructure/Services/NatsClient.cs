using MarketDepth.Application.Contracts;
using STAN.Client;

namespace MarketDepth.Infrastructure.Services
{
    public class NatsClient : IMessageBusPublisher
    {
        private Config _config;
        private StanConnectionFactory _cf;
        private IStanConnection _c;

        public NatsClient(IOptions<Config> options)
        {
            _config = options.Value;
            _cf = new StanConnectionFactory();

            var opts = StanOptions.GetDefaultOptions();
            opts.NatsURL = _config.NatsUrl;
            _c = _cf.CreateConnection(_config.ClusterId, _config.ClientId, opts);
        }
        public async Task<string> Publish(string channel, byte[] data)
        {
            return await _c.PublishAsync(channel, data);
        }

        public class Config
        {
            public string ClusterId { get; set; }
            public string ClientId { get; set; }
            public string NatsUrl { get; set; }
        }
    }
}

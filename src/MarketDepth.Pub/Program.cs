using MarketDepth.Application.Contracts;
using MarketDepth.Infrastructure.Services;
using MarketDepth.Pub;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<NatsStreamingClient.Settings>(context.Configuration.GetSection("nats"));
        services.Configure<BinanceWebSocketClient.Settings>(context.Configuration.GetSection("Binance"));

        services.AddScoped<IMessageBusClient, NatsStreamingClient>();
        services.AddScoped<IExchangeSocketClient, BinanceWebSocketClient>();
        services.AddHostedService<DepthPublisher>();
    })
    .Build();

await host.RunAsync();

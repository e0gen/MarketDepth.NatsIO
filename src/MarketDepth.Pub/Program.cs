using MarketDepth.Application.Contracts;
using MarketDepth.Infrastructure.Services;
using MarketDepth.NatsIO.Pub;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<NatsClient.Config>(context.Configuration.GetSection("nats"));
        services.Configure<BinanceWebSocketClient.Config>(context.Configuration.GetSection("BinanceApi"));

        services.AddScoped<IMessageBusPublisher, NatsClient>();
        services.AddScoped<IExchangeSocketClient, BinanceWebSocketClient>();
        services.AddHostedService<DepthPublisher>();
    })
    .Build();

await host.RunAsync();

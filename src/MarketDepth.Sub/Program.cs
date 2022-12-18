using MarketDepth.Application.Contracts;
using MarketDepth.Infrastructure.Services;
using MarketDepth.Sub;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<NatsStreamingClient.Settings>(context.Configuration.GetSection("nats"));

        services.AddScoped<IMessageBusClient, NatsStreamingClient>();
        services.AddHostedService<DepthSubscriber>();
    })
    .Build();

await host.RunAsync();

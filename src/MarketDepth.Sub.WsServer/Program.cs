using MarketDepth.Application.Contracts;
using MarketDepth.Infrastructure.Services;
using MarketDepth.Infrastructure.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<NatsStreamingClient.Settings>(builder.Configuration.GetSection("nats"));
builder.Services.AddSingleton<IMessageBusClient, NatsStreamingClient>();

var app = builder.Build();

var symbol = builder.Configuration.GetVarFromEnvironment("SYMBOL") ??
    throw new Exception($"Environment variable required: SYMBOL");


var wsOptions = new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(120) };
app.UseWebSockets(wsOptions);

app.Map($"/public.depth.${symbol}", async (HttpContext context, CancellationToken ct,
    [FromServices] IMessageBusClient messageBus) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

        while (!ct.IsCancellationRequested)
        {
            await messageBus.Subscribe(async (data) =>
            {
                Console.WriteLine($"[Nats Event] Bytes recieved: {data.Length}");
                await webSocket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);


            }, ct);

            await Task.Delay(1000);
        }

        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "nats subscription stop", CancellationToken.None);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

app.Run();


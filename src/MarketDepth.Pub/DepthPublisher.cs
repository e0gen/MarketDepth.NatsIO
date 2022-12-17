using MarketDepth.Application.Contracts;
using MarketDepth.Domain.Models;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace MarketDepth.NatsIO.Pub
{
    public class DepthPublisher : BackgroundService
    {
        private readonly ILogger<DepthPublisher> _logger;
        private readonly IExchangeSocketClient _exchangeSocketClient;
        private readonly IMessageBusPublisher _messageBus;

        public DepthPublisher(ILogger<DepthPublisher> logger, 
            IExchangeSocketClient exchangeSocketClient,
            IMessageBusPublisher messageBus)
        {
            _logger = logger;
            _exchangeSocketClient = exchangeSocketClient;
            _messageBus = messageBus;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            var guid = _exchangeSocketClient.ConnectToDepthWebSocket(async (depthData) =>
            {
                Console.WriteLine($"Depth Event: {depthData.EventType} {
                    depthData.EventTime} {depthData.Symbol} {depthData.UpdateId}");

                using (MemoryStream ms = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(ms, depthData);
                    await _messageBus.Publish(depthData.Symbol, ms.ToArray());
                }
            });

            while (!ct.IsCancellationRequested)
            {
                _logger.LogInformation("Publisher running at: {time}", DateTimeOffset.Now);

                await Task.Delay(1000, ct);
            }

            _exchangeSocketClient.CloseWebSocketInstance(guid);
        }
    }
}
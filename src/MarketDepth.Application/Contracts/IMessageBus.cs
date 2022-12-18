namespace MarketDepth.Application.Contracts
{
    public interface IMessageBusClient
    {
        string Publish(byte[] payload);
        Task<string> PublishAsync(byte[] payload);
        Task Subscribe(Action<byte[]> messageHandler, CancellationToken ct);
    }
}
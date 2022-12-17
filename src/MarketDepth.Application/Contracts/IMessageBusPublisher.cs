namespace MarketDepth.Application.Contracts
{
    public interface IMessageBusPublisher
    {
        Task<string> Publish(string channel, byte[] data);
    }
}
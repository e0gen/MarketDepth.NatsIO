namespace MarketDepth.Domain.Models
{
    public sealed class MarketEvent<T>
    {
        public string EventType { get; set; }
        public DateTime EventTime { get; set; }
        public T Data { get; set; }
    }
}

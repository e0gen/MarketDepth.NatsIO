namespace MarketDepth.Domain.Models
{
    public class DepthData
    {
        public string EventType { get; set; }
        public DateTime EventTime { get; set; }
        public string Symbol { get; set; }
        public long UpdateId { get; set; }
        public List<Quote> BidDepthDeltas { get; set; }
        public List<Quote> AskDepthDeltas { get; set; }
    }
}

namespace MarketDepth.Domain.Models
{
    public sealed class DepthUpdate
    {
        public string Symbol { get; set; }
        public long UpdateId { get; set; }
        public List<Quote> BidDepthDeltas { get; set; }
        public List<Quote> AskDepthDeltas { get; set; }
    }
}

namespace IBApi.Quotes
{
    public struct Quote
    {
        public double BidPrice { get; set; }
        public int BidSize { get; set; }
        public double AskPrice { get; set; }
        public int AskSize { get; set; }
        public double TradePrice { get; set; }
        public int TradeSize { get; set; }
    }
}
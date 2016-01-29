using IBApi.Contracts;

namespace IBApi.Orders
{
    public struct OrderParams
    {
        public Contract Contract { get; set; }
        public OrderAction OrderAction { get; set; }
        public OrderType OrderType { get; set; }
        public decimal? LimitPrice { get; set; }
        public decimal? StopPrice { get; set; }
        public int Quantity { get; set; }
    }
}

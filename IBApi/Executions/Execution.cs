using System;
using IBApi.Contracts;

namespace IBApi.Executions
{
    public struct Execution
    {
        public string Account { get; set; }
        public bool Buy { get; set; }
        public string Exchange { get; set; }
        public string Id { get; set; }
        public int OrderId { get; set; }
        public double AveragePrice { get; set; }
        public double Price { get; set; }
        public int CumQuantity { get; set; }
        public int Quantity { get; set; }
        public DateTime Time { get; set; }
        public Contract Contract { get; set; }
    }
}
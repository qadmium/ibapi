using System;
using System.Globalization;
using IBApi.Contracts;
using IBApi.Messages.Server;

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

        internal static Execution FromMessage(ExecutionDataMessage message, string account)
        {
            var executionTime = DateTime.ParseExact(message.ExecutionTime, "yyyyMMdd  HH:mm:ss", CultureInfo.InvariantCulture);

            var execution = new Execution
            {
                Account = account,
                Buy = message.Side == "BOT",
                Exchange = message.ExecutionExchange,
                Id = message.ExecutionId,
                OrderId = message.OrderId,
                Price = message.Price,
                AveragePrice = message.AveragePrice,
                CumQuantity = message.CumQty,
                Quantity = message.Shares,
                Time = executionTime,
                Contract = Contract.FromExecutionDataMessage(message)
            };

            return execution;
        }
    }
}
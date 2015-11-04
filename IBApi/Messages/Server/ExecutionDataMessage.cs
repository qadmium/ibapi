using System;
using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(11)]
    public struct ExecutionDataMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
        public int OrderId;
        public int ContractId;
        public string Symbol;
        public string SecurityType;
        public DateTime? Expiry;
        public double Strike;
        public string Right;
        public string Exchange;
        public string Currency;
        public string LocalSymbol;
        public string ExecutionId;
        public string ExecutionTime;
        public string Account;
        public string ExecutionExchange;
        public string Side;
        public int Shares;
        public double Price;
        public int PermId;
        public int ClientId;
        public int Liquidation;
        public int CumQty;
        public double AveragePrice;
    }
}
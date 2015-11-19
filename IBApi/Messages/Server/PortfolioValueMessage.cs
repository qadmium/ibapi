using System;
using IBApi.Serialization;

namespace IBApi.Messages.Server
{

    [IBSerializable(7)]
    struct PortfolioValueMessage : IServerMessage
    {
        public int Version;

        public int ContractId;
        public string Symbol;
        public string SecurityType;
        public DateTime? Expiry;
        public double Strike;
        public string Right;

        public string Multiplier;
        public string PrimaryExchange;

        public string Currency;
        public string LocalSymbol;

        public string TradingClass;

        public int Position;
        public double MarketPrice;
        public double MarketValue;
        public double AverageCost;
        public double UnrealizedPNL;
        public double RealizedPNL;

        public string AccountName;
    }
}

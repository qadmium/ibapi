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

        public double? Multiplier;
        public string PrimaryExchange;

        public string Contract;
        public string LocalSymbol;

        public int Position;
        public double MarketPrice;
        public double MarketValue;
        public double AverageCost;
        public double UnrealizedPNL;
        public double RealizedPNL;

        public string AccountName;
    }
}

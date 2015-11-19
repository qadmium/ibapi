using System;
using IBApi.Contracts;
using IBApi.Messages.Client.MessagesHelperExtensions;
using IBApi.Serialization;
using System.Collections.Generic;

namespace IBApi.Messages.Client
{
    public class UnderComp
    {
        public int ConId;
        public double Delta;
        public double Price;
    }

    [IBSerializable(1)]
    internal struct RequestMarketDataMessage : IClientMessage
    {
        public int Version;
        public int RequestId;
        public string Symbol;
        public string SecurityType;
        public DateTime? Expiry;
        public double? Strike;
        public string Right;
        public string Multiplier;
        public string Exchange;
        public string PrimaryExchange;
        public string Currency;
        public string LocalSymbol;
        public IEnumerable<IBagLeg> BagLegs;

        public bool ShouldSerializeBagLegs()
        {
            return SecurityType == "BAG";
        }

        public bool UnderCompPresentFlag;
        public UnderComp UnderCompProperty;

        public bool ShouldSerializeUnderCompProperty()
        {
            return UnderCompPresentFlag;
        }

        public string GenericTicks;
        public bool Snapshot;

        public static RequestMarketDataMessage FromContract(Contract contract)
        {
            return new RequestMarketDataMessage
            {
                Version = 8,
                Symbol = contract.Symbol,
                SecurityType = contract.SecurityType.ToString(),
                Expiry = contract.Expiry,
                Strike = contract.Strike,
                Right = contract.Call.ToRightString(),
                Multiplier = contract.AdditionalContractInfo.Multiplier,
                Exchange = contract.AdditionalContractInfo.Exchange,
                PrimaryExchange = contract.AdditionalContractInfo.PrimaryExchange,
                Currency = contract.AdditionalContractInfo.Currency,
                LocalSymbol = contract.LocalSymbol,
                GenericTicks = "100,101,104,106,107,165,221,225", // magic string from TWS C++ API
                Snapshot = false
            };
        }
    }
}

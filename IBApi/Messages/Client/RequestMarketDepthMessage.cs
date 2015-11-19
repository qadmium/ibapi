using System;
using IBApi.Contracts;
using IBApi.Messages.Client.MessagesHelperExtensions;
using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IBSerializable(10)]
    internal struct RequestMarketDepthMessage : IClientMessage
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
        public string Currency;
        public string LocalSymbol;
        public int NumberOfRows;

        public static RequestMarketDepthMessage FromContract(Contract contract)
        {
            return new RequestMarketDepthMessage
            {
                Version = 3,
                Symbol = contract.Symbol,
                SecurityType = contract.SecurityType.ToString(),
                Expiry = contract.Expiry,
                Strike = contract.Strike,
                Right = contract.Call.ToRightString(),
                Multiplier = contract.AdditionalContractInfo.Multiplier,
                Exchange = contract.AdditionalContractInfo.Exchange,
                Currency = contract.AdditionalContractInfo.Currency,
                LocalSymbol = contract.LocalSymbol,
                NumberOfRows = 100 //Max number
            };
        }
    }
}
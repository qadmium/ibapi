using System;
using System.Collections.Generic;
using System.Diagnostics;
using IBApi.Messages.Client;
using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(10)]
    [DebuggerDisplay("Symbol = {Symbol}, Currency = {Currency}, LocalSymbol = {LocalSymbol}, Type = {SecurityType}")]
    struct ContractDataMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
        public string Symbol;
        public string SecurityType;
        public DateTime? Expiry;
        public double Strike;
        public string Right;
        public string Exchange;
        public string Currency;
        public string LocalSymbol;
        public string MarketName;
        public string TradingClass;
        public int ContractId;
        public double MinTick;
        public string Multiplier;
        public string OrderTypes;
        public string ValidExchanges;
        public int PriceMagnifier;
        public int UnderCondId;
        public string LongName;
        public string PrimaryExchange;
        public string ContractMonth;
        public string Industry;
        public string Category;
        public string SubCategory;
        public string TimeZoneId;
        public string TradingHours;
        public string LiquidHours;
        public string EvRule;
        public double? EvMultiplier;
        public IEnumerable<TagValue> SecIdList;
    }
}

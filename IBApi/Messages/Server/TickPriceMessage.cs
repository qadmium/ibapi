using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    internal enum TickType
    {
        BidSize,
        Bid,
        Ask,
        AskSize,
        Last,
        LastSize,
        High,
        Low,
        Volume,
        Close,
        BidOptionComputation,
        AskOptionComputation,
        LastOptionComputation,
        ModelOption,
        Open,
        Low13Week,
        High13Week,
        Low26Week,
        High26Week,
        Low52Week,
        High52Week,
        AverageVolume,
        OpenInterest,
        OptionHistoricalVol,
        OptionImpliedVol,
        OptionBidExch,
        OptionAskExch,
        OptionCallOpenInterest,
        OptionPutOpenInterest,
        OptionCallVolume,
        OptionPutVolume,
        IndexFuturePremium,
        BidExch,
        AskExch,
        AuctionVolume,
        AuctionPrice,
        AuctionImbalance,
        MarkPrice,
        BidEfpComputation,
        AskEfpComputation,
        LastEfpComputation,
        OpenEfpComputation,
        HighEfpComputation,
        LowEfpComputation,
        CloseEfpComputation,
        LastTimestamp,
        Shortable,
        FundamentalRatios,
        RtVolume,
        Halted,
        NotSet
    }

    [IBSerializable(1)]
    internal struct TickPriceMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
        public TickType TickType;
        public double Price;
        public int Size;
        public int CanAutoExecute;
    }
}

using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    internal enum MarketDepthOperation
    {
        Insert,
        Update,
        Delete
    }

    [IBSerializable(13)]
    internal struct MarketDepthL2Message : IServerMessage
    {
        public int Version;
        public int RequestId;
        public int Position;
        public string MarketMaker;
        public MarketDepthOperation Operation;
        public bool BidSide;
        public double Price;
        public int Size;
    }
}
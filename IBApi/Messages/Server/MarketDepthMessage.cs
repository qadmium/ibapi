using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(12)]
    internal struct MarketDepthMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
        public int Position;
        public MarketDepthOperation Operation;
        public bool BidSide;
        public double Price;
        public int Size;
    }
}
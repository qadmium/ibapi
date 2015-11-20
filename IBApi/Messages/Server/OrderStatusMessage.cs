using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(3)]
    public struct OrderStatusMessage : IServerMessage
    {
        public int Version;
        public int OrderId;
        public string Status;
        public int Filled;
        public int Remaining;
        public double AverageFillPrice;
        public int PermId;
        public int ParentId;
        public double LastFillPrice;
        public int ClientId;
        public string WhyHeld;
    }
}
using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IBSerializable(11)]
    public struct RequestCancelMarketDepth : IClientMessage
    {
        public int Version;
        public int RequestId;

        public static RequestCancelMarketDepth Default
        {
            get { return new RequestCancelMarketDepth { Version = 3 }; }
        }
    }
}
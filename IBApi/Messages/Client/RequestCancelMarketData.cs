using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IbSerializable(2)]
    public struct RequestCancelMarketData : IClientMessage
    {
        public int Version;
        public int RequestId;

        public static RequestCancelMarketData Default
        {
            get { return new RequestCancelMarketData {Version = 3}; }
        }
    }
}
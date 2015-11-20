using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(53)]
    internal struct OpenOrderEndMessage : IServerMessage
    {
        public int Version;
    }
}

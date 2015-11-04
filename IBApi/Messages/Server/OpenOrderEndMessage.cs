using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(53)]
    internal struct OpenOrderEndMessage : IServerMessage
    {
        public int Version;
    }
}

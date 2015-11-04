using IBApi.Serialization;

namespace IBApi.Messages.Server
{

    [IBSerializable(46)]
    internal struct TickStringMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
        public int TickType;
        public string Value;
    }
}
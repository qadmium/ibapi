using IBApi.Serialization;

namespace IBApi.Messages.Server
{

    [IBSerializable(9)]
    struct NextValidIdMessage : IServerMessage
    {
        public int Version;
        public int OrderId;
    }
}

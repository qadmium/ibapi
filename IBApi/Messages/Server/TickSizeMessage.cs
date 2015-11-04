using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(2)]
    struct TickSizeMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
        public int TickTypeInt;
        public int Size;
    }

}

using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(8)]
    struct AccountUpdateTimeMessage : IServerMessage
    {
        public int Version;

        public string AccountTime;
    }
}

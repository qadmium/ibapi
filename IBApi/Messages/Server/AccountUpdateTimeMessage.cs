using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(8)]
    struct AccountUpdateTimeMessage : IServerMessage
    {
        public int Version;

        public string AccountTime;
    }
}

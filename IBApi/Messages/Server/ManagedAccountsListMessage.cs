using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(15)]
    struct ManagedAccountsListMessage : IServerMessage
    {
        public int Version;

        public string AccountsList;
    }
}

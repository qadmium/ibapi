using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(15)]
    struct ManagedAccountsListMessage : IServerMessage
    {
        public int Version;

        public string AccountsList;
    }
}

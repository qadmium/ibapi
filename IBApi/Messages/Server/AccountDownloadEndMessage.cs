using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(54)]
    struct AccountDownloadEndMessage : IServerMessage
    {
        public int Version;

        public string AccountName;
    }
}

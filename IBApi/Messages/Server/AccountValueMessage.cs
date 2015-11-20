using IBApi.Serialization;

namespace IBApi.Messages.Server
{

    [IbSerializable(6)]
    struct AccountValueMessage : IServerMessage
    {
        public int Version;

        public string Key;

        public string Value;

        public string Currency;

        public string AccountName;
    }
}

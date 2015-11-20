using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IbSerializable(6)]
    internal struct RequestAccountUpdatesMessage : IClientMessage
    {
        public RequestAccountUpdatesMessage(string accountName)
        {
            Version = 2;
            AccountCode = accountName;
            Subscribe = true;
        }

        public readonly int Version;
        public readonly bool Subscribe;
        public readonly string AccountCode;
    }
}

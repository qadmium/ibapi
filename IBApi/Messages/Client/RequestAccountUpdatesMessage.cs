using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IBSerializable(6)]
    internal struct RequestAccountUpdatesMessage : IClientMessage
    {
        public RequestAccountUpdatesMessage(string accountName)
        {
            Version = 2;
            AccountCode = accountName;
            Subscribe = true;
        }

        public int Version;
        public bool Subscribe;
        public string AccountCode;
    }
}

using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IbSerializable(17)]
    internal struct RequestManagedAccountsListMessage : IClientMessage
    {
        public static RequestManagedAccountsListMessage Default
        {
            get
            {
                var result = new RequestManagedAccountsListMessage {Version = 1};
                return result;
            }
        }

        public int Version;
    }
}

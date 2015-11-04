using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IBSerializable(7)]
    internal struct RequestExecutionsMessage : IClientMessage
    {
        public static RequestExecutionsMessage Default
        {
            get
            {
                var result = new RequestExecutionsMessage {Version = 3};
                return result;
            }
        }

        public int Version;

        public int RequestId;

        public int FilterByClientId;

        public string FilterByAccountCode;

        public string FilterByTime;
        
        public string FilterBySymbol;

        public string FilterBySecurityType;

        public string FilterByExchange;

        public string FilterBySide;
    }
}

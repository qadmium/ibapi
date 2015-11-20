using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IbSerializable(15)]
    internal struct RequestAutoOpenOrdersMessage : IClientMessage
    {
        public static RequestAutoOpenOrdersMessage Default
        {
            get
            {
                var result = new RequestAutoOpenOrdersMessage {Version = 1, AutoBind = true};
                return result;
            }
        }

        public int Version;

        public bool AutoBind;        
    }
}

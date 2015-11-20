using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IbSerializable(8)]
    internal struct RequestNextIdMessage : IClientMessage
    {
        public int Version;
        public int NumOfIds;

        public static RequestNextIdMessage Default
        {
            get
            {
                return new RequestNextIdMessage
                {
                    Version = 1,
                    NumOfIds = 1
                };
            }
        }
    }
}
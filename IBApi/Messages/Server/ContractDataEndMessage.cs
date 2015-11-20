using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(52)]
    struct ContractDataEndMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
    }
}

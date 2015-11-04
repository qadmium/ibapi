using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(52)]
    struct ContractDataEndMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
    }
}

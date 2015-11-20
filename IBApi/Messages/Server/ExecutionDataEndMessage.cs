using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(55)]
    struct ExecutionDataEndMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
    }
}
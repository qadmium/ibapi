using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(55)]
    struct ExecutionDataEndMessage : IServerMessage
    {
        public int Version;
        public int RequestId;
    }
}
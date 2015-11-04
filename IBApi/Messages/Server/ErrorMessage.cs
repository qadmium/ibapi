using IBApi.Errors;
using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IBSerializable(4)]
    struct ErrorMessage : IServerMessage
    {
        public int Version;

        public int RequestId;

        public ErrorCode ErrorCode;

        public string Message;
    }
}

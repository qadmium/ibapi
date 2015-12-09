using IBApi.Errors;
using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(4)]
    struct ErrorMessage : IServerMessage
    {
        public int Version;

        public int RequestId;

        public ErrorCode ErrorCode;

        public string Message;

        public override string ToString()
        {
            return string.Format("RequestId: {0}. ErrorCode: {1}. {2}", this.RequestId, this.ErrorCode, this.Message);
        }
    }
}

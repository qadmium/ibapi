using System;
using IBApi.Errors;

namespace IBApi.Exceptions
{
    [Serializable]
    public sealed class IBException : ApplicationException
    {
        public IBException(string message, ErrorCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; private set; }
    }
}

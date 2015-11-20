using System;
using IBApi.Errors;

namespace IBApi.Exceptions
{
    [Serializable]
    public sealed class IbException : ApplicationException
    {
        public IbException(string message, ErrorCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; private set; }
    }
}

using System;

namespace IBApi.Exceptions
{
    [Serializable]
    public class BlockedConnectionException : Exception
    {
        public BlockedConnectionException(string message)
            : base(message)
        {
        }
    }
}

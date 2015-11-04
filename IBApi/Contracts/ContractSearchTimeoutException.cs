using System;

namespace IBApi.Contracts
{
    [Serializable]
    public class ContractSearchTimeoutException : Exception
    {
        public ContractSearchTimeoutException(string message)
            : base(message)
        {
        }
    }
}
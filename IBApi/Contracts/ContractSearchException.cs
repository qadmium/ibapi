using System;
using IBApi.Errors;

namespace IBApi.Contracts
{
    [Serializable]
    public class ContractSearchException : Exception, IEquatable<ContractSearchException>
    {
        public ContractSearchException(string value)
            : base(value)
        {
        }

        public ContractSearchException(Error value)
            : base(value.Message)
        {
            Error = value;
        }

        public bool Equals(ContractSearchException other)
        {
            return other != null && other.Error == Error;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ContractSearchException);
        }

        public override int GetHashCode()
        {
            return Error.GetHashCode();
        }

        public Error Error { get; private set; }
    }
}
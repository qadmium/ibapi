using System;

namespace IBApi.Errors
{
    public struct Error : IEquatable<Error>
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
        public int RequestId { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is Error) && Equals((Error)obj);
        }

        public bool Equals(Error other)
        {
            return Code == other.Code && Message == other.Message && RequestId == other.RequestId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Code;
                hashCode = (hashCode * 397) ^ (Message != null ? Message.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ RequestId;
                return hashCode;
            }
        }

        public static bool operator ==(Error first, Error second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Error first, Error second)
        {
            return !(first == second);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} RequestId:{2}", Code, Message, RequestId);
        }
    }
}
using System;

namespace IBApi.Serialization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    internal sealed class IbSerializable : Attribute
    {
        public IbSerializable(int typeId)
        {
            this.IbTypeId = typeId;
        }

        public int IbTypeId { get; private set; }
    }
}
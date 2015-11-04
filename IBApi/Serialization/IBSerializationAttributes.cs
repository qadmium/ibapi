using System;

namespace IBApi.Serialization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    internal sealed class IBSerializable : Attribute
    {
        public IBSerializable(int typeId)
        {
            IBTypeId = typeId;
        }

        public int IBTypeId { get; private set; }
    }
}

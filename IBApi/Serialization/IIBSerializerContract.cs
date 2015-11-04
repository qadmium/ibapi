using System;
using System.Diagnostics.Contracts;
using System.IO;
using IBApi.Messages;

namespace IBApi.Serialization
{
    // ReSharper disable once InconsistentNaming
    [ContractClassFor(typeof(IIBSerializer))]
    internal abstract class IIBSerializerContract : IIBSerializer
    {
        public abstract T ReadMessageWithoutId<T>(Stream stream) where T : IMessage, new();

        public IMessage ReadServerMessage(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);
            Contract.Requires<ArgumentException>(stream.CanRead);
            
            Contract.Ensures(Contract.Result<IMessage>() != null);

            return null;
        }

        public abstract IMessage ReadClientMessage(Stream stream);
        public abstract void Write(IMessage message, Stream stream);
    }
}
using System.Diagnostics.Contracts;
using System.IO;
using IBApi.Messages;

namespace IBApi.Serialization
{
    [ContractClass(typeof(IIBSerializerContract))]
    internal interface IIBSerializer
    {
        T ReadMessageWithoutId<T>(Stream stream) where T : IMessage, new();
        IMessage ReadServerMessage(Stream stream);
        IMessage ReadClientMessage(Stream stream);
        void Write(IMessage message, Stream stream);
    }
}
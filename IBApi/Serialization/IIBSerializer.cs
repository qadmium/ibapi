using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Messages;

namespace IBApi.Serialization
{
    [ContractClass(typeof(IIbSerializerContract))]
    internal interface IIbSerializer
    {
        Task<T> ReadMessageWithoutId<T>(FieldsStream stream, CancellationToken cancellationToken) where T : IMessage, new();
        Task<IMessage> ReadServerMessage(FieldsStream stream, CancellationToken cancellationToken);
        Task<IMessage> ReadClientMessage(FieldsStream stream, CancellationToken cancellationToken);
        Task Write(IMessage message, FieldsStream stream, CancellationToken cancellationToken);
    }
}
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Messages;

namespace IBApi.Serialization
{
    // ReSharper disable once InconsistentNaming
    [ContractClassFor(typeof(IIbSerializer))]
    internal abstract class IIbSerializerContract : IIbSerializer
    {
        public abstract Task<T> ReadMessageWithoutId<T>(FieldsStream stream, CancellationToken cancellationToken) where T : IMessage, new();

        public Task<IMessage> ReadServerMessage(FieldsStream stream, CancellationToken cancellationToken)
        {
            Contract.Requires(stream != null);
            return null;
        }
        public abstract Task<IMessage> ReadClientMessage(FieldsStream stream, CancellationToken cancellationToken);
        public abstract Task Write(IMessage message, FieldsStream stream, CancellationToken cancellationToken);
    }
}
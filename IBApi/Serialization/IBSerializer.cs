using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Messages;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Serialization.SerializerExtensions;

namespace IBApi.Serialization
{
    internal class IBSerializer : IIbSerializer
    {
        public IBSerializer()
        {
            assemblyTypes = GetSerializableTypes(Assembly.GetExecutingAssembly());
        }

        public IBSerializer(Assembly assemblyWithAdditionalTypes)
            : this()
        {
            assemblyTypes = assemblyTypes.Concat(GetSerializableTypes(assemblyWithAdditionalTypes)).ToArray();
        }
        private static IEnumerable<Type> GetSerializableTypes(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(type => Attribute.IsDefined(type, typeof(IBSerializable)))
                .ToArray();
        }

        private Type GetType<T>(int typeId) where T : IMessage
        {
            return assemblyTypes
                .Where(type => typeof(T).IsAssignableFrom(type))
                .Single(type => type.GetCustomAttributes(false).OfType<IBSerializable>().Single().IBTypeId == typeId);
        }

        private readonly IEnumerable<Type> assemblyTypes;
        public async Task<T> ReadMessageWithoutId<T>(FieldsStream stream, CancellationToken cancellationToken) where T : IMessage, new()
        {
            return (T)await stream.ReadObject(typeof(T), cancellationToken);
        }

        public async Task<IMessage> ReadServerMessage(FieldsStream stream, CancellationToken cancellationToken)
        {
            var typeId = await stream.ReadTypeIdFromStream(cancellationToken);

            Type typeInStream;

            try
            {
                typeInStream = GetType<IServerMessage>(typeId);
            }
            catch (InvalidOperationException)
            {
                Trace.WriteLine(string.Format("Unrecognized type in stream: {0}", typeId));
                throw;
            }

            return (IMessage)await stream.ReadObject(typeInStream, cancellationToken);
        }

        public async Task<IMessage> ReadClientMessage(FieldsStream stream, CancellationToken cancellationToken)
        {
            var typeId = await stream.ReadTypeIdFromStream(cancellationToken);

            var typeInStream = GetType<IClientMessage>(typeId);

            return (IMessage)await stream.ReadObject(typeInStream, cancellationToken);
        }

        public async Task Write(IMessage message, FieldsStream stream, CancellationToken cancellationToken)
        {
            await message.Serialize(stream, cancellationToken);
        }
    }
}

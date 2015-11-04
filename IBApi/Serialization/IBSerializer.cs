using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using IBApi.Messages;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Serialization.SerializerExtensions;

namespace IBApi.Serialization
{
    internal class IBSerializer : IIBSerializer
    {
        public IBSerializer()
        {
            assemblyTypes = GetSerializableTypes(Assembly.GetExecutingAssembly());
        }

        public IBSerializer(Assembly assemblyWithAdditionalTypes)
            : this()
        {
            assemblyTypes = assemblyTypes.Concat(GetSerializableTypes(assemblyWithAdditionalTypes));
        }

        public T ReadMessageWithoutId<T>(Stream stream) where T : IMessage, new()
        {
            return (T)stream.ReadObject(typeof(T));
        }

        public IMessage ReadServerMessage(Stream stream)
        {
            var typeId = stream.ReadTypeIdFromStream();

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

            return (IMessage)stream.ReadObject(typeInStream);
        }

        public IMessage ReadClientMessage(Stream stream)
        {
            var typeId = stream.ReadTypeIdFromStream();

            var typeInStream = GetType<IClientMessage>(typeId);

            return (IMessage)stream.ReadObject(typeInStream);
        }

        public void Write(IMessage message, Stream stream)
        {
            message.Serialize(stream);
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
    }
}

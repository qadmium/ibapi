using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class IBSerializerTests
    {
        private readonly IBSerializer serializer = new IBSerializer(Assembly.GetExecutingAssembly());

        [TestMethod]
        public void TestWriteWithoutTypeId()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            var message = new MessageWithoutTypeId {Field = 42};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

            var result = new byte[3];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(3, stream.Length);

            var expected = Encoding.ASCII.GetBytes(message.Field.ToString() + char.MinValue);
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestWriteWithTypeId()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            var message = new MessageWithTypeId {Field = 42, Field2 = "42"};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

            var result = new byte[11];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1001.ToString() + char.MinValue +
                message.Field + char.MinValue +
                message.Field2 + char.MinValue);
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public async void TestReadWithoutTypeId()
        {
            var message = new ServerMessageWithoutTypeId {Field = 42};
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);
            stream.Seek(0, SeekOrigin.Begin);

            var result = await this.serializer.ReadMessageWithoutId<ServerMessageWithoutTypeId>(fieldsStream,
                CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public async void EnsureExceptionThrownOnUnknownMessage()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            var buffer = Encoding.ASCII.GetBytes("323232" + char.MinValue);
            stream.Write(buffer, 0, buffer.Length);
            stream.Seek(0, SeekOrigin.Begin);

            await this.serializer.ReadServerMessage(fieldsStream, CancellationToken.None);
        }

        [TestMethod]
        public async void TestReadWithTypeId()
        {
            var message = new ServerMessageWithTypeId {Field = 42, Field2 = "42"};
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);
            stream.Seek(0, SeekOrigin.Begin);

            var result = await this.serializer.ReadServerMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public async void TestSerializeEmptyEnumerable()
        {
            var message = new MessageWithEnumerable();
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);
            stream.Seek(0, SeekOrigin.Begin);

            var result = await this.serializer.ReadServerMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public async void TestSerializeNonEmptyEnumerable()
        {
            var message = new MessageWithEnumerable {Container = new[] {1, 2, 3}};
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);
            stream.Seek(0, SeekOrigin.Begin);

            var result = await this.serializer.ReadServerMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public async void TestConditionalSerializeNonEmptyEnumerable()
        {
            var message = new MessageWithConditionalEnumerable {Ver = 2, Container = new[] {1, 2, 3}};
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);
            stream.Seek(0, SeekOrigin.Begin);

            var result = await this.serializer.ReadServerMessage(fieldsStream, CancellationToken.None);

            var expected = new MessageWithConditionalEnumerable {Ver = 2, Container = null};
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async void TestConditionalSerializeNonEmptyEnumerable2()
        {
            var message = new MessageWithConditionalEnumerable {Ver = 4, Container = new[] {1, 2, 3}};
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            await this.serializer.Write(message, fieldsStream, CancellationToken.None);
            stream.Seek(0, SeekOrigin.Begin);

            var result = await this.serializer.ReadServerMessage(fieldsStream, CancellationToken.None);

            var expected = new MessageWithConditionalEnumerable {Ver = 4, Container = new[] {1, 2, 3}};
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EnsureThatStaticaPropertiesNotSerialized()
        {
            var message = new MessageWithStaticProperty();
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            this.serializer.Write(message, fieldsStream, CancellationToken.None);
            stream.Seek(0, SeekOrigin.Begin);

            var result = new byte[5];
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(1005.ToString() + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public async void TestSerializationWithNullableWithValue()
        {
            var message = new MessageWithNullableProperty {Field = 42};
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);
            stream.Seek(0, SeekOrigin.Begin);

            var result = await this.serializer.ReadServerMessage(fieldsStream, CancellationToken.None);

            var expected = new MessageWithNullableProperty {Field = 42};
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async void TestSerializationWithNullableWithoutValue()
        {
            var message = new MessageWithNullableProperty();
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);
            stream.Seek(0, SeekOrigin.Begin);

            var result = await this.serializer.ReadServerMessage(fieldsStream, CancellationToken.None);

            var expected = new MessageWithNullableProperty();
            Assert.AreEqual(expected, result);
        }

        private class MessageWithoutTypeId : IClientMessage
        {
            public int Field;
        }

        [IBSerializable(1001)]
        private class MessageWithTypeId : IClientMessage
        {
            public int Field;

            public string Field2;
        }

        private class ServerMessageWithoutTypeId : IServerMessage, IComparable<ServerMessageWithoutTypeId>
        {
            public int Field;

            public int CompareTo(ServerMessageWithoutTypeId other)
            {
                return this.Field.CompareTo(other.Field);
            }

            protected bool Equals(ServerMessageWithoutTypeId other)
            {
                return this.Field == other.Field;
            }

            public override int GetHashCode()
            {
                return this.Field;
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof (ServerMessageWithoutTypeId)
                       && this.CompareTo((ServerMessageWithoutTypeId) obj) == 0;
            }
        }

        [IBSerializable(1002)]
        private class ServerMessageWithTypeId : IServerMessage, IComparable<ServerMessageWithTypeId>
        {
            public int Field;

            public string Field2;

            public int CompareTo(ServerMessageWithTypeId other)
            {
                var result = this.Field.CompareTo(other.Field);

                return result == 0 ? this.Field2.CompareTo(other.Field2) : result;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (this.Field*397) ^ (this.Field2 != null ? this.Field2.GetHashCode() : 0);
                }
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof (ServerMessageWithTypeId)
                       && this.CompareTo((ServerMessageWithTypeId) obj) == 0;
            }

            public override string ToString()
            {
                return this.Field + " " + this.Field2;
            }
        }

        [IBSerializable(1003)]
        private class MessageWithEnumerable : IServerMessage
        {
            public IEnumerable<int> Container;

            public MessageWithEnumerable()
            {
                this.Container = new List<int>();
            }

            private bool Equals(MessageWithEnumerable other)
            {
                return this.Container.SequenceEqual(other.Container);
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof (MessageWithEnumerable)
                       && this.Equals((MessageWithEnumerable) obj);
            }

            public override int GetHashCode()
            {
                return this.Container.GetHashCode();
            }
        }

        [IBSerializable(1004)]
        private class MessageWithConditionalEnumerable : IServerMessage
        {
            public IEnumerable<int> Container;
            public int Ver;

            public bool ShouldSerializeContainer()
            {
                return this.Ver > 3;
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof (MessageWithConditionalEnumerable)
                       && this.Equals((MessageWithConditionalEnumerable) obj);
            }

            private bool Equals(MessageWithConditionalEnumerable other)
            {
                return this.Ver == other.Ver
                       &&
                       (this.Container == null && other.Container == null ||
                        this.Container.SequenceEqual(other.Container));
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (this.Ver*397) ^ (this.Container != null ? this.Container.GetHashCode() : 0);
                }
            }
        }

        [IBSerializable(1005)]
        private class MessageWithStaticProperty : IServerMessage
        {
            public static int SomeProp = 42;

            private bool Equals(MessageWithEnumerable other)
            {
                return true;
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof (MessageWithEnumerable);
            }

            public override int GetHashCode()
            {
                return 0;
            }
        }

        [IBSerializable(1006)]
        private class MessageWithNullableProperty : IServerMessage
        {
            public int? Field;

            protected bool Equals(MessageWithNullableProperty other)
            {
                return this.Field == other.Field;
            }

            public override int GetHashCode()
            {
                return this.Field.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof (MessageWithNullableProperty);
            }
        }
    }
}
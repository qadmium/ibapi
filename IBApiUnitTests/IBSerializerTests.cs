using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IBApi.Serialization;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class IBSerializerTests
    {
        private readonly IBSerializer serializer= new IBSerializer(Assembly.GetExecutingAssembly());

        class MessageWithoutTypeId : IClientMessage
        {
            public int Field;
        }

        [TestMethod]
        public void TestWriteWithoutTypeId()
        {
            var stream = new MemoryStream();

            var message = new MessageWithoutTypeId() {Field = 42};

            serializer.Write(message, stream);

            var result = new byte[3];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(3, stream.Length);

            var expected = Encoding.ASCII.GetBytes(message.Field.ToString() + char.MinValue);
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [IBSerializable(1001)]
        class MessageWithTypeId : IClientMessage
        {
            public int Field;

            public string Field2;
        }

        [TestMethod]
        public void TestWriteWithTypeId()
        {
            var stream = new MemoryStream();

            var message = new MessageWithTypeId() { Field = 42, Field2 = "42"};

            serializer.Write(message, stream);

            var result = new byte[11];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1001.ToString() + char.MinValue + 
                message.Field.ToString() + char.MinValue +
                message.Field2 + char.MinValue);
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        class ServerMessageWithoutTypeId : IServerMessage, IComparable<ServerMessageWithoutTypeId>
        {
            protected bool Equals(ServerMessageWithoutTypeId other)
            {
                return Field == other.Field;
            }

            public override int GetHashCode()
            {
                return Field;
            }

            public int Field;

            public int CompareTo(ServerMessageWithoutTypeId other)
            {
                return Field.CompareTo(other.Field);
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof(ServerMessageWithoutTypeId)
                       && CompareTo((ServerMessageWithoutTypeId)obj) == 0;
            }
        }

        [TestMethod]
        public void TestReadWithoutTypeId()
        {
            var message = new ServerMessageWithoutTypeId() {Field = 42};
            var stream = new MemoryStream();

            serializer.Write(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            var result = serializer.ReadMessageWithoutId<ServerMessageWithoutTypeId>(stream);

            Assert.AreEqual(message, result);
        }

        [IBSerializable(1002)]
        class ServerMessageWithTypeId : IServerMessage, IComparable<ServerMessageWithTypeId>
        {
            public override int GetHashCode()
            {
                unchecked
                {
                    return (Field * 397) ^ (Field2 != null ? Field2.GetHashCode() : 0);
                }
            }

            public int Field;

            public string Field2;

            public int CompareTo(ServerMessageWithTypeId other)
            {
                var result = Field.CompareTo(other.Field);

                return result == 0 ? Field2.CompareTo(other.Field2) : result;
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof(ServerMessageWithTypeId)
                       && CompareTo((ServerMessageWithTypeId)obj) == 0;
            }

            public override string ToString()
            {
                return Field.ToString() + " " + Field2;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EnsureExceptionThrownOnUnknownMessage()
        {
            var stream = new MemoryStream();

            var buffer = Encoding.ASCII.GetBytes("323232" + char.MinValue);
            stream.Write(buffer, 0, buffer.Length);
            stream.Seek(0, SeekOrigin.Begin);

            var result = serializer.ReadServerMessage(stream);
        }

        [TestMethod]
        public void TestReadWithTypeId()
        {
            var message = new ServerMessageWithTypeId() { Field = 42, Field2 = "42"};
            var stream = new MemoryStream();

            serializer.Write(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            var result = serializer.ReadServerMessage(stream);

            Assert.AreEqual(message, result);
        }

        [IBSerializable(1003)]
        class MessageWithEnumerable : IServerMessage
        {
            public MessageWithEnumerable()
            {
                Container = new List<int>();
            }

            public IEnumerable<int> Container;

            private bool Equals(MessageWithEnumerable other)
            {
                return Container.SequenceEqual(other.Container);
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof(MessageWithEnumerable)
                       && Equals((MessageWithEnumerable)obj);
            }

            public override int GetHashCode()
            {
                return Container.GetHashCode();
            }
        }

        [TestMethod]
        public void TestSerializeEmptyEnumerable()
        {
            var message = new MessageWithEnumerable();
            var stream = new MemoryStream();

            serializer.Write(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            var result = serializer.ReadServerMessage(stream);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializeNonEmptyEnumerable()
        {
            var message = new MessageWithEnumerable(){Container = new[]{1, 2, 3}};
            var stream = new MemoryStream();

            serializer.Write(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            var result = serializer.ReadServerMessage(stream);

            Assert.AreEqual(message, result);
        }

        [IBSerializable(1004)]
        class MessageWithConditionalEnumerable : IServerMessage
        {
            public int Ver;

            public IEnumerable<int> Container;

            public bool ShouldSerializeContainer()
            {
                return Ver > 3;
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof(MessageWithConditionalEnumerable)
                       && Equals((MessageWithConditionalEnumerable)obj);
            }

            private bool Equals(MessageWithConditionalEnumerable other)
            {
                return Ver == other.Ver 
                    && ((Container == null && other.Container == null) || Container.SequenceEqual(other.Container));
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Ver * 397) ^ (Container != null ? Container.GetHashCode() : 0);
                }
            }
        }

        [TestMethod]
        public void TestConditionalSerializeNonEmptyEnumerable()
        {
            var message = new MessageWithConditionalEnumerable() { Ver = 2, Container = new[] { 1, 2, 3 } };
            var stream = new MemoryStream();

            serializer.Write(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            var result = serializer.ReadServerMessage(stream);

            var expected = new MessageWithConditionalEnumerable() { Ver = 2, Container = null };
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConditionalSerializeNonEmptyEnumerable2()
        {
            var message = new MessageWithConditionalEnumerable() { Ver = 4, Container = new[] { 1, 2, 3 } };
            var stream = new MemoryStream();

            serializer.Write(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            var result = serializer.ReadServerMessage(stream);

            var expected = new MessageWithConditionalEnumerable() { Ver = 4, Container = new[] { 1, 2, 3 } };
            Assert.AreEqual(expected, result);
        }

        [IBSerializable(1005)]
        class MessageWithStaticProperty : IServerMessage
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

        [TestMethod]
        public void EnsureWhatStaticaPropertiesNotSerialized()
        {
            var message = new MessageWithStaticProperty();
            
            var stream = new MemoryStream();

            serializer.Write(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            var result = new byte[5];
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(1005.ToString() + char.MinValue);
            
            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [IBSerializable(1006)]
        class MessageWithNullableProperty : IServerMessage
        {
            public int? Field;

            protected bool Equals(MessageWithNullableProperty other)
            {
                return Field == other.Field;
            }

            public override int GetHashCode()
            {
                return Field.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof(MessageWithNullableProperty);
            }
        }

        [TestMethod]
        public void TestSerializationWithNullableWithValue()
        {
            var message = new MessageWithNullableProperty() { Field = 42 };
            var stream = new MemoryStream();

            serializer.Write(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            var result = serializer.ReadServerMessage(stream);

            var expected = new MessageWithNullableProperty() { Field = 42 };
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestSerializationWithNullableWithoutValue()
        {
            var message = new MessageWithNullableProperty();
            var stream = new MemoryStream();

            serializer.Write(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            var result = serializer.ReadServerMessage(stream);

            var expected = new MessageWithNullableProperty();
            Assert.AreEqual(expected, result);
        }

    }
}

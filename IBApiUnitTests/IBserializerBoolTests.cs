using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IBApi.Messages.Client;
using IBApi.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class IBSerializerBoolTests
    {
        private readonly IBSerializer serializer = new IBSerializer(Assembly.GetExecutingAssembly());

        [IBSerializable(1007)]
        class MessageWithIBBoolNullable : IClientMessage
        {
            protected bool Equals(MessageWithIBBoolNullable other)
            {
                return Field.Equals(other.Field);
            }

            public override int GetHashCode()
            {
                return Field.GetHashCode();
            }

            public bool? Field;

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof(MessageWithIBBoolNullable))
                {
                    return false;
                }

                return Equals((MessageWithIBBoolNullable) obj);
            }
        }

        [TestMethod]
        public void TestSerializationWithIBBoolNullableTrue()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBoolNullable { Field = true };

            serializer.Write(message, stream);

            var result = new byte[7];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1007.ToString() + char.MinValue +
                "1" + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestDeserializationWithIBBoolNullableTrue()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBoolNullable { Field = true };

            serializer.Write(message, stream);

            stream.Seek(0, SeekOrigin.Begin);
            var result = serializer.ReadClientMessage(stream);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializationWithIBBoolNullableFalse()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBoolNullable { Field = false };

            serializer.Write(message, stream);

            var result = new byte[7];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1007.ToString() + char.MinValue +
                "0" + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestDeserializationWithIBBoolNullableFalse()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBoolNullable { Field = false };

            serializer.Write(message, stream);

            stream.Seek(0, SeekOrigin.Begin);
            var result = serializer.ReadClientMessage(stream);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializationWithIBBoolNullableNull()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBoolNullable { Field = null };

            serializer.Write(message, stream);

            var result = new byte[6];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1007.ToString() + char.MinValue + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestDeserializationWithIBBoolNullableNull()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBoolNullable { Field = null };

            serializer.Write(message, stream);

            stream.Seek(0, SeekOrigin.Begin);
            var result = serializer.ReadClientMessage(stream);

            Assert.AreEqual(message, result);
        }

        [IBSerializable(1008)]
        class MessageWithIBBool : IClientMessage
        {
            protected bool Equals(MessageWithIBBool other)
            {
                return Field.Equals(other.Field);
            }

            public override int GetHashCode()
            {
                return Field.GetHashCode();
            }

            public bool Field;

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof(MessageWithIBBool))
                {
                    return false;
                }

                return Equals((MessageWithIBBool)obj);
            }
        }

        [TestMethod]
        public void TestSerializationWithIBBoolTrue()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBool { Field = true };

            serializer.Write(message, stream);

            var result = new byte[7];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1008.ToString() + char.MinValue +
                "1" + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestDeserializationWithIBBoolTrue()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBool { Field = true };

            serializer.Write(message, stream);

            stream.Seek(0, SeekOrigin.Begin);
            var result = serializer.ReadClientMessage(stream);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializationWithIBBoolFalse()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBool { Field = false };

            serializer.Write(message, stream);

            var result = new byte[7];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1008.ToString() + char.MinValue +
                "0" + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestDeserializationWithIBBoolFalse()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBBool { Field = false };

            serializer.Write(message, stream);

            stream.Seek(0, SeekOrigin.Begin);
            var result = serializer.ReadClientMessage(stream);

            Assert.AreEqual(message, result);
        }
    }
}

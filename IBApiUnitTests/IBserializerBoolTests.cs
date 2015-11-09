using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Messages.Client;
using IBApi.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class IBSerializerBoolTests
    {
        private readonly IBSerializer serializer = new IBSerializer(Assembly.GetExecutingAssembly());

        [TestMethod]
        public void TestSerializationWithIBBoolNullableTrue()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBoolNullable {Field = true};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

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
        public async Task TestDeserializationWithIBBoolNullableTrue()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBoolNullable {Field = true};

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);

            stream.Seek(0, SeekOrigin.Begin);
            var result = await this.serializer.ReadClientMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializationWithIBBoolNullableFalse()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBoolNullable {Field = false};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

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
        public async Task TestDeserializationWithIBBoolNullableFalse()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBoolNullable {Field = false};

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);

            stream.Seek(0, SeekOrigin.Begin);
            var result = await this.serializer.ReadClientMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializationWithIBBoolNullableNull()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBoolNullable {Field = null};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

            var result = new byte[6];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1007.ToString() + char.MinValue + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public async Task TestDeserializationWithIBBoolNullableNull()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBoolNullable {Field = null};

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);

            stream.Seek(0, SeekOrigin.Begin);
            var result = await this.serializer.ReadClientMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializationWithIBBoolTrue()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBool {Field = true};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

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
        public async Task TestDeserializationWithIBBoolTrue()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBool {Field = true};

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);

            stream.Seek(0, SeekOrigin.Begin);
            var result = await this.serializer.ReadClientMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializationWithIBBoolFalse()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBool {Field = false};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

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
        public async Task TestDeserializationWithIBBoolFalse()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBBool {Field = false};

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);

            stream.Seek(0, SeekOrigin.Begin);
            var result = await this.serializer.ReadClientMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [IBSerializable(1007)]
        private class MessageWithIBBoolNullable : IClientMessage
        {
            public bool? Field;

            private bool Equals(MessageWithIBBoolNullable other)
            {
                return this.Field.Equals(other.Field);
            }

            public override int GetHashCode()
            {
                return this.Field.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof (MessageWithIBBoolNullable))
                {
                    return false;
                }

                return this.Equals((MessageWithIBBoolNullable) obj);
            }
        }

        [IBSerializable(1008)]
        private class MessageWithIBBool : IClientMessage
        {
            public bool Field;

            private bool Equals(MessageWithIBBool other)
            {
                return this.Field.Equals(other.Field);
            }

            public override int GetHashCode()
            {
                return this.Field.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof (MessageWithIBBool))
                {
                    return false;
                }

                return this.Equals((MessageWithIBBool) obj);
            }
        }
    }
}
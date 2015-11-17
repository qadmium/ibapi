using System;
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
    public class IbSerializerEnumTests
    {
        private readonly IBSerializer serializer = new IBSerializer(Assembly.GetExecutingAssembly());

        [TestMethod]
        public void TestEnumSerialization()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithEnum {Field = TestEnum.Two};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

            var result = new byte[7];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                2007.ToString() + char.MinValue +
                "2" + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestNullEnumSerialization()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithNullableEnum { Field = null };

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

            var result = new byte[6];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                2008.ToString() + char.MinValue + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public async Task TestEnumDeserialization()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithEnum {Field = TestEnum.Three};

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);

            stream.Seek(0, SeekOrigin.Begin);

            var deserializedMessage =
                (MessageWithEnum) await this.serializer.ReadClientMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, deserializedMessage);
        }

        private enum TestEnum
        {
            Zero,
            One,
            Two,
            Three
        }

        [IBSerializable(2007)]
        private class MessageWithEnum : IClientMessage, IEquatable<MessageWithEnum>
        {
            public TestEnum Field;

            public bool Equals(MessageWithEnum other)
            {
                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.Field.Equals(other.Field);
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as MessageWithEnum);
            }

            public override int GetHashCode()
            {
                return this.Field.GetHashCode();
            }
        }

        [IBSerializable(2008)]
        private class MessageWithNullableEnum : IClientMessage, IEquatable<MessageWithNullableEnum>
        {
            public TestEnum? Field;

            public bool Equals(MessageWithNullableEnum other)
            {
                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.Field.Equals(other.Field);
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as MessageWithNullableEnum);
            }

            public override int GetHashCode()
            {
                return this.Field.GetHashCode();
            }
        }
    }
}
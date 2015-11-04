using System;
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
    public class IBSerializerEnumTests
    {
        private readonly IBSerializer serializer = new IBSerializer(Assembly.GetExecutingAssembly());

        enum TestEnum
        {
            Zero,
            One,
            Two,
            Three
        }

        [IBSerializable(2007)]
        class MessageWithEnum : IClientMessage, IEquatable<MessageWithEnum>
        {
            public TestEnum Field;

            public override bool Equals(object obj)
            {
                return Equals(obj as MessageWithEnum);
            }

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

                return Field.Equals(other.Field);
            }

            public override int GetHashCode()
            {
                return Field.GetHashCode();
            }
        }

        [TestMethod]
        public void TestEnumSerialization()
        {
            var stream = new MemoryStream();

            var message = new MessageWithEnum { Field = TestEnum.Two };

            serializer.Write(message, stream);

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
        public void TestEnumDeserialization()
        {
            var stream = new MemoryStream();

            var message = new MessageWithEnum { Field = TestEnum.Three };

            serializer.Write(message, stream);

            stream.Seek(0, SeekOrigin.Begin);

            var deserializedMessage = (MessageWithEnum)serializer.ReadClientMessage(stream);

            Assert.AreEqual(message, deserializedMessage);
        }
    }
}

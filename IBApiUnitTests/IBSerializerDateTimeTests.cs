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
    public class IBSerializerDateTimeTests
    {
        private readonly IBSerializer serializer = new IBSerializer(Assembly.GetExecutingAssembly());

        [IBSerializable(1009)]
        class MessageWithIBDateTime : IClientMessage
        {
            protected bool Equals(MessageWithIBDateTime other)
            {
                return Field.Equals(other.Field);
            }

            public override int GetHashCode()
            {
                return Field.GetHashCode();
            }

            public DateTime? Field;

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof(MessageWithIBDateTime))
                {
                    return false;
                }

                return Equals((MessageWithIBDateTime)obj);
            }
        }

        [TestMethod]
        public void TestSerializationWithIBDateTimeWithDate()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBDateTime { Field = new DateTime(2013, 11, 20) };

            serializer.Write(message, stream);

            var result = new byte[14];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1009.ToString() + char.MinValue +
                "20131120" + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestDeserializationWithIBDateTimeWithDate()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBDateTime { Field = new DateTime(2013, 11, 20) };

            serializer.Write(message, stream);

            stream.Seek(0, SeekOrigin.Begin);
            var result = serializer.ReadClientMessage(stream);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializationWithIBDateTimeNull()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBDateTime { Field = null };

            serializer.Write(message, stream);

            var result = new byte[6];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1009.ToString() + char.MinValue + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public void TestDeserializationWithIBDateTimeNull()
        {
            var stream = new MemoryStream();

            var message = new MessageWithIBDateTime { Field = null };

            serializer.Write(message, stream);

            stream.Seek(0, SeekOrigin.Begin);
            var result = serializer.ReadClientMessage(stream);

            Assert.AreEqual(message, result);
        }
    }
}

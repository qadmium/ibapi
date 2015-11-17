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
    public class IbSerializerDateTimeTests
    {
        private readonly IBSerializer serializer = new IBSerializer(Assembly.GetExecutingAssembly());

        [TestMethod]
        public void TestSerializationWithIBDateTimeWithDate()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBDateTime {Field = new DateTime(2013, 11, 20)};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

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
        public async Task TestDeserializationWithIBDateTimeWithDate()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBDateTime {Field = new DateTime(2013, 11, 20)};

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);

            stream.Seek(0, SeekOrigin.Begin);
            var result = await this.serializer.ReadClientMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [TestMethod]
        public void TestSerializationWithIBDateTimeNull()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBDateTime {Field = null};

            this.serializer.Write(message, fieldsStream, CancellationToken.None);

            var result = new byte[6];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(result, 0, result.Length);

            Assert.AreEqual(result.Length, stream.Length);

            var expected = Encoding.ASCII.GetBytes(
                1009.ToString() + char.MinValue + char.MinValue);

            Assert.IsTrue(expected.SequenceEqual(result));
        }

        [TestMethod]
        public async Task TestDeserializationWithIBDateTimeNull()
        {
            var stream = new MemoryStream();
            var fieldsStream = new FieldsStream(stream);
            var message = new MessageWithIBDateTime {Field = null};

            await this.serializer.Write(message, fieldsStream, CancellationToken.None);

            stream.Seek(0, SeekOrigin.Begin);
            var result = await this.serializer.ReadClientMessage(fieldsStream, CancellationToken.None);

            Assert.AreEqual(message, result);
        }

        [IBSerializable(1009)]
        private class MessageWithIBDateTime : IClientMessage
        {
            public DateTime? Field;

            protected bool Equals(MessageWithIBDateTime other)
            {
                return this.Field.Equals(other.Field);
            }

            public override int GetHashCode()
            {
                return this.Field.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof (MessageWithIBDateTime))
                {
                    return false;
                }

                return this.Equals((MessageWithIBDateTime) obj);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using IBApi.Connection;
using IBApi.Errors;
using IBApi.Messages;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    internal class IBSerializerMock : IIBSerializer, IDisposable
    {
        public IBSerializerMock()
        {
            WritedMessages = new List<IMessage>();
        }

        public T ReadMessageWithoutId<T>(Stream stream) where T : IMessage, new()
        {
            throw new NotImplementedException();
        }

        public IMessage ReadServerMessage(Stream stream)
        {
            @event.WaitOne();

            if (disconnected)
            {
                throw new IOException("Emulated disconnect");
            }

            Assert.IsNotNull(nextServerMessage);
            var result = nextServerMessage;
            nextServerMessage = null;
            return result;
        }

        public IMessage ReadClientMessage(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Write(IMessage message, Stream stream)
        {
            WritedMessages.Add(message);
        }

        public List<IMessage> WritedMessages { get; set; }

        public void SendMessageFromServer(IServerMessage message)
        {
            nextServerMessage = message;
            @event.Set();
        }

        public void EmulateDisconnect()
        {
            disconnected = true;
            @event.Set();
        }

        public void Dispose()
        {
            @event.Dispose();
        }

        private readonly AutoResetEvent @event = new AutoResetEvent(false);
        private IServerMessage nextServerMessage;
        private bool disconnected;
    }

    [TestClass]
    public class ConnectionTests
    {
        [TestInitialize]
        public void Init()
        {
            oldSynchronizationContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

            streamMock = new Mock<Stream>();
            serializerMock = new IBSerializerMock();
            connection = new Connection(streamMock.Object, serializerMock);
        }

        [TestCleanup]
        public void Cleanup()
        {
            connection.Dispose();
            serializerMock.Dispose();
            SynchronizationContext.SetSynchronizationContext(oldSynchronizationContext);
        }

        [TestMethod]
        public void EnsureWhatConnectionSendsMessages()
        {
            connection.Run();
            
            connection.SendMessage(new RequestAutoOpenOrdersMessage());

            Assert.AreEqual(1, serializerMock.WritedMessages.Count);
            Assert.IsInstanceOfType(serializerMock.WritedMessages[0], typeof(RequestAutoOpenOrdersMessage));
        }

        [TestMethod]
        public void EnsureWhatConnectionWillNotSendsMessagesAfterDisposing()
        {
            connection.Run();
            connection.Dispose();

            connection.SendMessage(new RequestAutoOpenOrdersMessage());

            Assert.AreEqual(0, serializerMock.WritedMessages.Count);
        }

        [TestMethod]
        public void EnsureWhatSubscriptionWorks()
        {
            var messageCallback = new Mock<Action<ErrorMessage>>();

            connection.Run();

            connection.Subscribe(message => true, messageCallback.Object, ImmediateScheduler.Instance);

            var sendedMessage = new ErrorMessage {ErrorCode = ErrorCode.DataInactiveButAvailable};

            serializerMock.SendMessageFromServer(sendedMessage);

            Delay();

            messageCallback.Verify(
                callback => callback(It.Is((ErrorMessage message) => message.ErrorCode == sendedMessage.ErrorCode)),
                Times.Once);
        }

        [TestMethod]
        public void EnsureConnectionReturnsDifferentRequestIds()
        {
            Assert.IsTrue(connection.NextRequestId() != connection.NextRequestId());
        }

        private static void Delay()
        {
            Thread.Sleep(300);
        }

        private Mock<Stream> streamMock;
        private Connection connection;
        private IBSerializerMock serializerMock;
        private SynchronizationContext oldSynchronizationContext;
    }
}

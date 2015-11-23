using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
    internal class IbSerializerMock : IIbSerializer
    {
        private TaskCompletionSource<IMessage> readTaskCompletionSource = new TaskCompletionSource<IMessage>();

        public IbSerializerMock()
        {
            this.WritedMessages = new List<IMessage>();
        }

        public List<IMessage> WritedMessages { get; private set; }

        public Task<T> ReadMessageWithoutId<T>(FieldsStream stream, CancellationToken cancellationToken)
            where T : IMessage, new()
        {
            throw new NotImplementedException();
        }

        public async Task<IMessage> ReadServerMessage(FieldsStream stream, CancellationToken cancellationToken)
        {
            var message = await this.readTaskCompletionSource.Task;
            this.readTaskCompletionSource = new TaskCompletionSource<IMessage>();
            return message;
        }

        public Task<IMessage> ReadClientMessage(FieldsStream stream, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

#pragma warning disable 1998
        public async Task Write(IMessage message, FieldsStream stream, CancellationToken cancellationToken)
#pragma warning restore 1998
        {
            this.WritedMessages.Add(message);
        }

        public void SendMessageFromServer(IServerMessage message)
        {
            this.readTaskCompletionSource.SetResult(message);
        }
    }

    [TestClass]
    public class ConnectionTests
    {
        private Connection connection;
        private IbSerializerMock serializerMock;

        [TestInitialize]
        public void Init()
        {
            this.serializerMock = new IbSerializerMock();
            this.connection = new Connection(new FieldsStream(new MemoryStream()), this.serializerMock);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.connection.Dispose();
        }

        [TestMethod]
        public void EnsureThatConnectionSendsMessages()
        {
            this.connection.SendMessage(new RequestAutoOpenOrdersMessage());

            Assert.AreEqual(1, this.serializerMock.WritedMessages.Count);
            Assert.IsInstanceOfType(this.serializerMock.WritedMessages[0], typeof (RequestAutoOpenOrdersMessage));
        }

        [TestMethod]
        public void EnsureThatConnectionWillNotSendsMessagesAfterDisposing()
        {
            this.connection.Dispose();

            this.connection.SendMessage(new RequestAutoOpenOrdersMessage());

            Assert.AreEqual(0, this.serializerMock.WritedMessages.Count);
        }

        [TestMethod]
        public void EnsureThatSubscriptionWorks()
        {
            var messageCallback = new Mock<Action<ErrorMessage>>();

            this.connection.Subscribe(message => true, messageCallback.Object);
            this.connection.ReadMessagesAndDispatch();

            var sendedMessage = new ErrorMessage {ErrorCode = ErrorCode.DataInactiveButAvailable};

            this.serializerMock.SendMessageFromServer(sendedMessage);

            messageCallback.Verify(
                callback => callback(It.Is((ErrorMessage message) => message.ErrorCode == sendedMessage.ErrorCode)),
                Times.Once);
        }
    }
}
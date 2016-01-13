using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using IBApi;
using IBApi.Connection;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using Moq;

namespace IBApiUnitTests
{
    internal sealed class ConnectionMock : IConnection
    {
        private readonly Mock<Action<IClientMessage>> sendMessageVerifier = new Mock<Action<IClientMessage>>();
        private readonly HashSet<ISubscription> subscriptions = new HashSet<ISubscription>();

        public void Dispose()
        {
        }

        public void SendMessage(IClientMessage message)
        {
            this.sendMessageVerifier.Object(message);
        }

        public IDisposable Subscribe<T>(Func<T, bool> condition, Action<T> callback)
        {
            var subscription = new Subscription<T>(condition, callback, this.subscriptions);
            this.subscriptions.Add(subscription);
            return subscription;
        }

        public IDisposable Subscribe<T>(Action<T> callback)
        {
            return this.Subscribe((T message) => true, callback);
        }

        public event EventHandler<DisconnectedEventArgs> OnDisconnect = delegate {};

        public void SendMessageToClient(IServerMessage message)
        {
            foreach (var subscription in this.subscriptions.ToList())
            {
                subscription.OnMessage(message);
            }
        }

        public void EnsureThatClientSentMessage<TValue>(Expression<Func<TValue, bool>> match, Func<Times> times)
            where TValue : IClientMessage
        {
            this.sendMessageVerifier.Verify(action => action(It.Is(match)), times);
        }
    }

    internal sealed class ConnectionHelper : IDisposable
    {
        public const int RequestId = 1;
        private readonly ConnectionMock connectionMock = new ConnectionMock();
        private readonly Mock<IIdsDispenser> dispenser = new Mock<IIdsDispenser>();

        public ConnectionHelper()
        {
            this.dispenser.Setup(dispenser => dispenser.NextOrderId())
                .Returns(RequestId);

            this.dispenser.Setup(dispenser => dispenser.NextRequestId())
                .Returns(RequestId);
        }

        public void Dispose()
        {
            this.connectionMock.Dispose();
        }

        public IConnection Connection()
        {
            return this.connectionMock;
        }

        public IIdsDispenser Dispenser()
        {
            return this.dispenser.Object;
        }

        public void SendMessage(IServerMessage message)
        {
            this.connectionMock.SendMessageToClient(message);
        }

        public void EnsureThatMessageSended<TValue>(Expression<Func<TValue, bool>> match, Func<Times> times)
            where TValue : IClientMessage
        {
            this.connectionMock.EnsureThatClientSentMessage(match, times);
        }
    }
}
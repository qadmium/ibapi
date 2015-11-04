using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IBApi;
using IBApi.Connection;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using Moq;

namespace IBApiUnitTests
{
    internal sealed class ConnectionMock : IConnection
    {
        public void Dispose()
        {
            subscriptions.Dispose();
        }

        public void Run()
        {
            Running = true;
        }

        public bool Running { get; private set; }

        public void SendMessage(IClientMessage message)
        {
            sendMessageVerifier.Object(message);
        }

        public int NextRequestId()
        {
            return ConnectionHelper.RequestId;
        }

        public IDisposable Subscribe<T>(Func<T, bool> condition, Action<T> callback, TaskScheduler scheduler)
        {
            var subscription = new Subscription<T>(condition, callback, subscriptions);
            subscriptions.Add(subscription, ImmediateScheduler.Instance);
            return subscription;
        }

        public IDisposable Subscribe<T>(Func<T, bool> condition, Action<T> callback)
        {
            return Subscribe(condition, callback, null);
        }

        public IDisposable Subscribe<T>(Action<T> callback)
        {
            return Subscribe((T message) => true, callback, null);
        }

        public void SendMessageToClient(IServerMessage message)
        {
            subscriptions.DispatchMessage(message);
        }

        public void EnsureThatClientSentMessage<TValue>(Expression<Func<TValue, bool>> match, Func<Times> times)
            where TValue : IClientMessage
        {
            sendMessageVerifier.Verify(action => action(It.Is(match)), times);
        }

        private readonly Mock<Action<IClientMessage>> sendMessageVerifier = new Mock<Action<IClientMessage>>();
        private readonly Subscriptions subscriptions = new Subscriptions();
    }

    internal sealed class ConnectionHelper : IDisposable
    {
        public IConnection Connection()
        {
            return connectionMock;
        }

        public void SendMessage(IServerMessage message)
        {
            connectionMock.SendMessageToClient(message);
        }

        public void EnsureThatMessageSended<TValue>(Expression<Func<TValue, bool>> match, Func<Times> times) where TValue : IClientMessage
        {
            connectionMock.EnsureThatClientSentMessage(match, times);
        }

        public const int RequestId = 1;
        private readonly ConnectionMock connectionMock = new ConnectionMock();
        
        public void Dispose()
        {
            connectionMock.Dispose();
        }
    }
}

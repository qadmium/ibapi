using System;
using IBApi.Messages.Server;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Connection
{
    internal sealed class Subscription<T> : ISubscription
    {
        public Subscription(Func<T, bool> condition, Action<T> callback, Subscriptions subscriptions)
        {
            CodeContract.Requires(condition != null);
            CodeContract.Requires(callback != null);
            CodeContract.Requires(subscriptions != null);

            this.condition = condition;
            this.callback = callback;
            this.subscriptions = subscriptions;
        }

        public void OnMessage(IServerMessage message)
        {
            if (!(message is T))
            {
                return;
            }

            var typedMessage = (T) message;
            if (condition(typedMessage))
            {
                callback(typedMessage);
            }
        }

        public void Dispose()
        {
            subscriptions.Remove(this);
        }

        private readonly Func<T, bool> condition;
        private readonly Action<T> callback;
        private readonly Subscriptions subscriptions;
    }
}

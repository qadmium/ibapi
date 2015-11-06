using System;
using System.Collections.Generic;
using IBApi.Messages.Server;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Connection
{
    internal sealed class Subscription<T> : ISubscription
    {
        private readonly Action<T> callback;
        private readonly Func<T, bool> condition;
        private readonly HashSet<ISubscription> subscriptions;

        public Subscription(Func<T, bool> condition, Action<T> callback, HashSet<ISubscription> subscriptions)
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
            if (this.condition(typedMessage))
            {
                this.callback(typedMessage);
            }
        }

        public void Dispose()
        {
            this.subscriptions.Remove(this);
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Messages.Server;

namespace IBApi.Connection
{
    class Subscriptions : IDisposable
    {
        public void Add(ISubscription subscription, TaskScheduler targetScheduler)
        {
            subscriptions.TryAdd(subscription, targetScheduler);
        }

        public void Remove(ISubscription subscription)
        {
            TaskScheduler value;
            subscriptions.TryRemove(subscription, out value);
        }

        public void DispatchMessage(IServerMessage message)
        {
            foreach (var subscriptionPair in subscriptions)
            {
                var scheduler = subscriptionPair.Value;
                var subscription = subscriptionPair.Key;

                Task.Factory.StartNew(() => subscription.OnMessage(message), cancellationTokenSource.Token,
                    TaskCreationOptions.None, scheduler);
            }        
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            foreach (var subscription in subscriptions.Keys)
            {
                subscription.Dispose();
            }
        }

        private readonly ConcurrentDictionary<ISubscription, TaskScheduler> subscriptions =
            new ConcurrentDictionary<ISubscription, TaskScheduler>();

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    }
}

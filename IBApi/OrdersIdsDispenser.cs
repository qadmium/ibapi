using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Messages.Client;
using IBApi.Messages.Server;

namespace IBApi
{
    internal class OrdersIdsDispenser : IOrdersIdsDispenser
    {
        private readonly IConnection connection;
        private readonly IDisposable subscription;
        private readonly ConcurrentQueue<TaskCompletionSource<int>> queue = new ConcurrentQueue<TaskCompletionSource<int>>();

        public OrdersIdsDispenser(IConnection connection)
        {
            Contract.Requires(connection != null);
            this.connection = connection;
            this.subscription = connection.Subscribe((NextValidIdMessage message) => this.OnNextValidId(message));
        }

        private void OnNextValidId(NextValidIdMessage message)
        {
            TaskCompletionSource<int> taskCompletionSource = null;

            while (taskCompletionSource == null || !taskCompletionSource.TrySetResult(message.OrderId))
            {
                if (!this.queue.TryDequeue(out taskCompletionSource))
                {
                    return;
                }
            }
        }

        public Task<int> NextOrderId(CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<int>();
            cancellationToken.Register(() => taskCompletionSource.SetCanceled());

            this.queue.Enqueue(taskCompletionSource);
            this.connection.SendMessage(RequestNextIdMessage.Default);
            return taskCompletionSource.Task;
        }

        public void Dispose()
        {
            this.subscription.Dispose();
        }
    }
}
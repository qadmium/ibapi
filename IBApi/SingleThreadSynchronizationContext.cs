using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace IBApi
{
    internal sealed class SingleThreadSynchronizationContext : SynchronizationContext
    {
        private readonly CancellationToken cancellationToken;

        private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> queue =
            new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();

        public SingleThreadSynchronizationContext(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
            cancellationToken.Register(this.Complete);
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }

            if (this.cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new NotSupportedException("Synchronously sending is not supported.");
        }

        public void Run()
        {
            foreach (var workItem in this.queue.GetConsumingEnumerable())
            {
                workItem.Key(workItem.Value);
            }
        }

        public void Complete()
        {
            this.queue.CompleteAdding();
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace IBApi.Infrastructure
{
    internal sealed class Dispatcher
    {
        private readonly CancellationToken cancellationToken;

        private readonly ConcurrentDictionary<object, ConcurrentDictionary<Delegate, TaskScheduler>> events =
            new ConcurrentDictionary<object, ConcurrentDictionary<Delegate, TaskScheduler>>();

        private readonly Thread schedulerThread;
        private readonly TaskScheduler taskScheduler;

        public Dispatcher(TaskScheduler taskScheduler, CancellationToken cancellationToken, Thread schedulerThread)
        {
            this.taskScheduler = taskScheduler;
            this.cancellationToken = cancellationToken;
            this.schedulerThread = schedulerThread;
        }

        public TResult Dispatch<TResult>(Func<TResult> function)
        {
            if (Thread.CurrentThread == this.schedulerThread)
            {
                return function();
            }

            return
                Task.Factory.StartNew(function, this.cancellationToken, TaskCreationOptions.None, this.taskScheduler)
                    .Result;
        }

        public object RegisterEvent()
        {
            var obj = new object();
            this.events.GetOrAdd(obj, o => new ConcurrentDictionary<Delegate, TaskScheduler>());
            return obj;
        }

        public void AddHandler<T>(object @event, EventHandler<T> eventHandler) where T : EventArgs
        {
            if (eventHandler == null)
            {
                return;
            }

            var handlers = this.events.GetOrAdd(@event, o => new ConcurrentDictionary<Delegate, TaskScheduler>());
            handlers.AddOrUpdate(eventHandler, TaskScheduler.FromCurrentSynchronizationContext(), (h, s) => s);
        }

        public void RemoveHandler<T>(object @event, EventHandler<T> eventHandler) where T : EventArgs
        {
            var handlers = this.events.GetOrAdd(@event, o => new ConcurrentDictionary<Delegate, TaskScheduler>());
            TaskScheduler ts;
            handlers.TryRemove(eventHandler, out ts);
        }

        public void RaiseEvent<T>(object @event, object sender, T eventArgs) where T : EventArgs
        {
            var handlers = this.events.GetOrAdd(@event, o => new ConcurrentDictionary<Delegate, TaskScheduler>());

            foreach (var handlerSchedulerPair in handlers)
            {
                var copy = handlerSchedulerPair;

                if (handlerSchedulerPair.Value == null)
                {
                    Task.Run(() => copy.Key.DynamicInvoke(sender, eventArgs), CancellationToken.None);
                }
                else
                {
                    Task.Factory.StartNew(() => copy.Key.DynamicInvoke(sender, eventArgs), CancellationToken.None,
                        TaskCreationOptions.None, handlerSchedulerPair.Value);
                }
            }
        }
    }
}
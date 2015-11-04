using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Serialization;

namespace IBApi.Connection
{
    internal sealed class Connection : IConnection
    {
        public Connection(Stream stream, IIBSerializer serializer)
        {
            Contract.Requires<ArgumentNullException>(stream != null);
            Contract.Requires<ArgumentNullException>(serializer != null);
            this.stream = stream;
            this.serializer = serializer;
            currentScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public void Run()
        {
            Running = true;
            //pass cancellationToken to prevent running task on main thread
            Task.Factory.StartNew(ReadMessagesAndDispatch, new CancellationToken());
        }

        public bool Running { get; private set; }

        private void ReadMessagesAndDispatch()
        {
            try
            {
                while (!disposed)
                {
                    var message = serializer.ReadServerMessage(stream) as IServerMessage;
                    subscriptions.DispatchMessage(message);

                    Trace.TraceInformation("Received message {0}", message);
                }
            }
            catch (IOException e)
            {
                RethrowIfUnexpectedException(e);
            }
            catch (ObjectDisposedException e)
            {
                RethrowIfUnexpectedException(e);
            }
            catch
            {
                Trace.TraceError("Unhandled exception on messages reader");
                throw;
            }

            Trace.TraceInformation("Messages reader exited");
        }

        public void SendMessage(IClientMessage message)
        {
            if (disposed)
            {
                return;
            }

            Trace.TraceInformation("Sending message: {0}", message);
            serializer.Write(message, stream);
        }

        public int NextRequestId()
        {
            return nextRequestId++;
        }

        public IDisposable Subscribe<T>(Func<T, bool> condition, Action<T> callback, TaskScheduler scheduler)
        {
            var subscription = new Subscription<T>(condition, callback, subscriptions);
            subscriptions.Add(subscription, scheduler);
            return subscription;
        }

        public IDisposable Subscribe<T>(Func<T, bool> condition, Action<T> callback)
        {
            return Subscribe(condition, callback, currentScheduler);
        }

        public IDisposable Subscribe<T>(Action<T> callback)
        {
            return Subscribe(message => true, callback, currentScheduler);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            stream.Dispose();
            subscriptions.Dispose();
        }

        private void RethrowIfUnexpectedException(Exception e)
        {
            if (disposed)
            {
                return;
            }

            Trace.TraceError("Unexpected exception: {0}", e);
            throw e;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(stream != null);
            Contract.Invariant(serializer != null);
        }

        private readonly Stream stream;
        private readonly IIBSerializer serializer;
        private readonly Subscriptions subscriptions = new Subscriptions();
        private int nextRequestId;
        private volatile bool disposed;
        private readonly TaskScheduler currentScheduler;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Serialization;

namespace IBApi.Connection
{
    internal sealed class Connection : IConnection
    {
        private readonly CancellationTokenSource cts;
        private readonly IIbSerializer serializer;

        private readonly FieldsStream stream;

        private readonly HashSet<ISubscription> subscriptions = new HashSet<ISubscription>();

        public Connection(FieldsStream stream, IIbSerializer serializer)
        {
            Contract.Requires(stream != null);
            Contract.Requires(serializer != null);
            this.stream = stream;
            this.serializer = serializer;
            this.cts = new CancellationTokenSource();
        }

        public void SendMessage(IClientMessage message)
        {
            if (this.cts.Token.IsCancellationRequested)
            {
                return;
            }

            Trace.TraceInformation("Sending message: {0}", message);
            this.serializer.Write(message, this.stream, this.cts.Token);
        }

        public IDisposable Subscribe<T>(Func<T, bool> condition, Action<T> callback)
        {
            var subscription = new Subscription<T>(condition, callback, this.subscriptions);
            this.subscriptions.Add(subscription);
            return subscription;
        }

        public IDisposable Subscribe<T>(Action<T> callback)
        {
            return this.Subscribe(message => true, callback);
        }

        public void Dispose()
        {
            this.cts.Cancel();
            this.stream.Dispose();
        }

        public async void ReadMessagesAndDispatch()
        {
            Trace.TraceInformation("Started reading messages");
            try
            {
                var token = this.cts.Token;
                while (!token.IsCancellationRequested)
                {
                    var message = await this.serializer.ReadServerMessage(this.stream, token) as IServerMessage;
                    this.DispatchMessage(message);

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

        private void DispatchMessage(IServerMessage message)
        {
            foreach (var subscription in this.subscriptions.ToList())
            {
                subscription.OnMessage(message);
            }
        }

        private static void RethrowIfUnexpectedException(Exception e)
        {
            Trace.TraceError("Unexpected exception: {0}", e);
            throw e;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.stream != null);
            Contract.Invariant(this.serializer != null);
        }
    }
}
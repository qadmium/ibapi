using System;
using System.Diagnostics.Contracts;
using IBApi.Connection;
using IBApi.Messages.Server;

namespace IBApi
{
    internal class IdsDispenser : IIdsDispenser
    {
        private readonly IDisposable subscription;
        private int nextRequestId;
        private int nextOrderId;

        public IdsDispenser(IConnection connection)
        {
            Contract.Requires(connection != null);
            this.subscription = connection.Subscribe((NextValidIdMessage message) => this.OnNextValidId(message));
        }

        private void OnNextValidId(NextValidIdMessage message)
        {
            this.nextOrderId = message.ValidId;
        }

        public int NextOrderId()
        {
            return this.nextOrderId++;
        }

        public int NextRequestId()
        {
            return this.nextRequestId++;
        }

        public void Dispose()
        {
            this.subscription.Dispose();
        }
    }
}
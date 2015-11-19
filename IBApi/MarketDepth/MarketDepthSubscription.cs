using System;
using System.Collections.Generic;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Errors;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using CodeContract = System.Diagnostics.Contracts.Contract;
using ContractInvariantMethod = System.Diagnostics.Contracts.ContractInvariantMethodAttribute;

namespace IBApi.MarketDepth
{
    internal class MarketDepthSubscription : IDisposable
    {
        private readonly IConnection connection;
        private readonly MarketDepthUpdatesDispatcher marketDepthUpdatesDispatcher;
        private readonly IMarketDepthObserver observer;
        private readonly int requestId;
        private bool disposed;
        private ICollection<IDisposable> subscriptions;

        public MarketDepthSubscription(IConnection connection, IMarketDepthObserver observer, Contract contract)
        {
            CodeContract.Requires(connection != null);
            CodeContract.Requires(observer != null);

            this.connection = connection;
            this.observer = observer;
            this.requestId = connection.NextRequestId();
            this.marketDepthUpdatesDispatcher = new MarketDepthUpdatesDispatcher(observer);

            this.Subscribe();
            this.SendRequest(contract);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            this.SendCancelRequest();
            this.subscriptions.Unsubscribe();
        }

        private void SendRequest(Contract contract)
        {
            var request = RequestMarketDepthMessage.FromContract(contract);
            request.RequestId = this.requestId;
            this.connection.SendMessage(request);
        }

        private void SendCancelRequest()
        {
            var cancelRequest = RequestCancelMarketDepth.Default;
            cancelRequest.RequestId = this.requestId;

            this.connection.SendMessage(cancelRequest);
        }

        private void Subscribe()
        {
            this.subscriptions = new List<IDisposable>
            {
                this.connection.Subscribe((MarketDepthMessage message) => message.RequestId == this.requestId,
                    this.OnMarketDepth),
                this.connection.SubscribeForRequestErrors(this.requestId, this.OnError)
            };
        }

        private void OnMarketDepth(MarketDepthMessage message)
        {
            this.marketDepthUpdatesDispatcher.OnMarketDepth(message);
        }

        private void OnError(Error error)
        {
            this.observer.OnError(error);
        }

        [ContractInvariantMethod]
        private void ContractInvariant()
        {
            CodeContract.Invariant(this.connection != null);
        }
    }
}
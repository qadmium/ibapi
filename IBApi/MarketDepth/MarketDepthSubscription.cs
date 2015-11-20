using System;
using System.Collections.Generic;
using System.Threading;
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
        private bool disposed;
        private int requestId;
        private ICollection<IDisposable> subscriptions;

        public MarketDepthSubscription(IConnection connection, IIdsDispenser dispenser, IMarketDepthObserver observer,
            Contract contract)
        {
            CodeContract.Requires(connection != null);
            CodeContract.Requires(dispenser != null);
            CodeContract.Requires(observer != null);

            this.connection = connection;
            this.observer = observer;
            this.marketDepthUpdatesDispatcher = new MarketDepthUpdatesDispatcher(observer);

            this.Subscribe(dispenser, contract);
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

        private void SendRequest(Contract contract, int requestId)
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

        private async void Subscribe(IIdsDispenser dispenser, Contract contract)
        {
            this.requestId = await dispenser.NextId(CancellationToken.None);

            this.subscriptions = new List<IDisposable>
            {
                this.connection.Subscribe((MarketDepthMessage message) => message.RequestId == this.requestId,
                    this.OnMarketDepth),
                this.connection.SubscribeForRequestErrors(this.requestId, this.OnError)
            };

            this.SendRequest(contract, this.requestId);
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
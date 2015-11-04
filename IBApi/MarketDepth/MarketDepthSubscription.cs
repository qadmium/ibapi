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
        public MarketDepthSubscription(IConnection connection, IMarketDepthObserver observer, Contract contract)
        {
            CodeContract.Requires(connection != null);

            this.connection = connection;
            this.observer = observer;
            requestId = connection.NextRequestId();
            marketDepthUpdatesDispatcher = new MarketDepthUpdatesDispatcher(observer);

            Subscribe();
            SendRequest(contract);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            SendCancelRequest();
            subscriptions.Unsubscribe();
        }

        private void SendRequest(Contract contract)
        {
            var request = RequestMarketDepthMessage.FromContract(contract);
            request.RequestId = requestId;
            connection.SendMessage(request);
        }

        private void SendCancelRequest()
        {
            var cancelRequest = RequestCancelMarketDepth.Default;
            cancelRequest.RequestId = requestId;

            connection.SendMessage(cancelRequest);
        }

        private void Subscribe()
        {
            subscriptions = new List<IDisposable>
            {
                connection.Subscribe((MarketDepthMessage message) => message.RequestId == requestId, OnMarketDepth),
                connection.SubscribeForRequestErrors(requestId, OnError)
            };
        }

        private void OnMarketDepth(MarketDepthMessage message)
        {
            marketDepthUpdatesDispatcher.OnMarketDepth(message);
        }

        private void OnError(Error error)
        {
            observer.OnError(error);
        }

        [ContractInvariantMethod]
        private void ContractInvariant()
        {
            CodeContract.Invariant(connection != null);
        }

        private readonly IConnection connection;
        private readonly IMarketDepthObserver observer;
        private readonly int requestId;
        private readonly MarketDepthUpdatesDispatcher marketDepthUpdatesDispatcher;
        private ICollection<IDisposable> subscriptions;
        private bool disposed;
    }
}
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

namespace IBApi.Quotes
{
    internal class QuoteSubscription : IDisposable
    {
        private readonly IConnection connection;
        private readonly IQuoteObserver observer;
        private int requestId;
        private ICollection<IDisposable> subscriptions;
        private bool disposed;
        private Quote quote;

        public QuoteSubscription(IConnection connection, IIdsDispenser dispenser, IQuoteObserver observer, Contract contract)
        {
            CodeContract.Requires(connection != null);
            CodeContract.Requires(observer != null);
            CodeContract.Requires(dispenser != null);

            this.connection = connection;
            this.observer = observer;

            this.Subscribe(contract, dispenser);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            this.subscriptions.Unsubscribe();
            this.SendCancelRequest();
        }

        private void Subscribe(Contract contract, IIdsDispenser dispenser)
        {
            this.requestId = dispenser.NextRequestId();

            this.subscriptions = new List<IDisposable>
            {
                this.connection.Subscribe((TickPriceMessage message) => message.RequestId == this.requestId,
                    this.OnTickPrice),
                this.connection.SubscribeForRequestErrors(this.requestId, this.OnError)
            };

            this.SendRequest(contract);
        }

        private void SendRequest(Contract contract)
        {
            var request = RequestMarketDataMessage.FromContract(contract);
            request.RequestId = this.requestId;
            this.connection.SendMessage(request);
        }

        private void SendCancelRequest()
        {
            var cancelRequest = RequestCancelMarketData.Default;
            cancelRequest.RequestId = this.requestId;
            this.connection.SendMessage(cancelRequest);
        }

        private void OnTickPrice(TickPriceMessage message)
        {
            this.UpdateQuote(message);
            this.observer.OnQuote(this.quote);
        }

        private void OnError(Error error)
        {
            this.observer.OnError(error);
        }

        private void UpdateQuote(TickPriceMessage tickPriceMessage)
        {
            switch (tickPriceMessage.TickType)
            {
                case TickType.Bid:
                    this.quote.BidPrice = tickPriceMessage.Price;
                    this.quote.BidSize = tickPriceMessage.Size;
                    return;

                case TickType.Ask:
                    this.quote.AskPrice = tickPriceMessage.Price;
                    this.quote.AskSize = tickPriceMessage.Size;
                    return;

                case TickType.Last:
                    this.quote.TradePrice = tickPriceMessage.Price;
                    this.quote.TradeSize = tickPriceMessage.Size;
                    return;
            }
        }

        [ContractInvariantMethod]
        private void ContractInvariant()
        {
            CodeContract.Invariant(this.connection != null);
        }
    }
}
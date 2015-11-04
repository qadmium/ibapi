using System;
using System.Collections.Generic;
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
        public QuoteSubscription(IConnection connection, IQuoteObserver observer, Contract contract)
        {
            CodeContract.Requires(connection != null);
            CodeContract.Requires(observer != null);

            this.connection = connection;
            this.observer = observer;
            requestId = connection.NextRequestId();

            subscriptions = Subscribe();
            SendRequest(contract);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            subscriptions.Unsubscribe();
            SendCancelRequest();
        }

        private ICollection<IDisposable> Subscribe()
        {
            return new List<IDisposable>
            {
                connection.Subscribe((TickPriceMessage message) => message.RequestId == requestId, OnTickPrice),
                connection.SubscribeForRequestErrors(requestId, OnError),
            };
        }

        private void SendRequest(Contract contract)
        {
            var request = RequestMarketDataMessage.FromContract(contract);
            request.RequestId = requestId;
            connection.SendMessage(request);
        }

        private void SendCancelRequest()
        {
            var cancelRequest = RequestCancelMarketData.Default;
            cancelRequest.RequestId = requestId;
            connection.SendMessage(cancelRequest);
        }

        private void OnTickPrice(TickPriceMessage message)
        {
            UpdateQuote(message);
            observer.OnQuote(quote);
        }

        private void OnError(Error error)
        {
            observer.OnError(error);
        }

        private void UpdateQuote(TickPriceMessage tickPriceMessage)
        {
            switch (tickPriceMessage.TickType)
            {
                case TickType.Bid:
                    quote.BidPrice = tickPriceMessage.Price;
                    quote.BidSize = tickPriceMessage.Size;
                    return;

                case TickType.Ask:
                    quote.AskPrice = tickPriceMessage.Price;
                    quote.AskSize = tickPriceMessage.Size;
                    return;

                case TickType.Last:
                    quote.TradePrice = tickPriceMessage.Price;
                    quote.TradeSize = tickPriceMessage.Size;
                    return;
            }
        }

        [ContractInvariantMethod]
        private void ContractInvariant()
        {
            CodeContract.Invariant(connection != null);
        }

        private readonly IConnection connection;
        private readonly IQuoteObserver observer;
        private readonly int requestId;
        private readonly ICollection<IDisposable> subscriptions;
        private bool disposed;
        private Quote quote;
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using IBApi.Connection;
using IBApi.Errors;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Contracts
{
    internal sealed class SyncFindContractOperation : IDisposable
    {
        public SyncFindContractOperation(IConnection connection)
        {
            CodeContract.Requires<ArgumentNullException>(connection != null);
            this.connection = connection;
        }

        public Contract ResultFor(SearchRequest request, int millisecondsTimeout)
        {
            CodeContract.Requires(!Finished);
            CodeContract.Ensures(Finished);

            var requestId = connection.NextRequestId();
            Subscribe(requestId);
            SendRequest(request, requestId);

            var eventWasSet = @event.WaitOne(millisecondsTimeout);
            subscriptions.Unsubscribe();
            
            if (!eventWasSet)
            {
                Finish();
                throw new ContractSearchTimeoutException("Search interrupted by timeout");
            }

            if (receivedError != null)
            {
                throw new ContractSearchException(receivedError.Value);
            }

            if (contract == null)
            {
                throw new ContractSearchException("Nothing found");
            }

            return contract.Value;
        }

        public bool Finished
        {
            get { return finished; }
        }

        private void SendRequest(SearchRequest request, int requestId)
        {
            var requestMessage = RequestContractDetailsMessage.Create(request);
            requestMessage.RequestId = requestId;
            connection.SendMessage(requestMessage);
        }

        private void Subscribe(int requestId)
        {
            subscriptions = new List<IDisposable>
            {
                connection.Subscribe(((ContractDataMessage message) => message.RequestId == requestId && !finished), OnContractDataMessage, ImmediateScheduler.Instance),
                connection.Subscribe(((ContractDataEndMessage message) => message.RequestId == requestId && !Finished), OnContractDataEndMessage, ImmediateScheduler.Instance),
                connection.SubscribeForRequestErrors(requestId, OnContractDataErrorMessage, ImmediateScheduler.Instance)
            };
        }

        public void Dispose()
        {
            @event.Dispose();
        }

        private void OnContractDataMessage(ContractDataMessage message)
        {
            contract = Contract.FromContractDataMessage(message);
            Finish();
        }

        private void OnContractDataEndMessage(ContractDataEndMessage message)
        {
            Finish();
        }

        private void OnContractDataErrorMessage(Error error)
        {
            if (Finished)
            {
                return;
            }
            receivedError = error;
            Finish();
        }

        private void Finish()
        {
            finished = true;
            @event.Set();
        }

        [System.Diagnostics.Contracts.ContractInvariantMethod]
        private void ObjectInvariant()
        {
            CodeContract.Invariant(@event != null);
            CodeContract.Invariant(connection != null);
        }

        private readonly ManualResetEvent @event = new ManualResetEvent(false);
        private readonly IConnection connection;
        private Contract? contract;
        private Error? receivedError;
        private ICollection<IDisposable> subscriptions;
        private volatile bool finished;
    }
}
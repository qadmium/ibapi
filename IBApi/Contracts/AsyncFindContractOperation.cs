using System;
using System.Collections.Generic;
using IBApi.Connection;
using IBApi.Errors;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Contracts
{
    internal sealed class AsyncFindContractOperation : IDisposable
    {
        public AsyncFindContractOperation(IConnection connection)
        {
            CodeContract.Requires(connection != null);

            this.connection = connection;
        }

        public void Start(IObserver<Contract> contractObserver, SearchRequest request)
        {
            CodeContract.Requires(contractObserver != null);
            CodeContract.Requires(Started == false);
            CodeContract.Ensures(Started == true);

            observer = contractObserver;
            var requestId = connection.NextRequestId();
            Subscribe(requestId);
            SendRequest(request, requestId);
        }

        public void Dispose()
        {
            subscriptions.Unsubscribe();
        }

        public bool Started
        {
            get
            {
                return subscriptions != null;
            }
        }

        private void Subscribe(int requestId)
        {
            subscriptions = new List<IDisposable>
            {

                connection.Subscribe((ContractDataMessage message) => message.RequestId == requestId, OnContractDataMessage),
                connection.Subscribe((ContractDataEndMessage message) => message.RequestId == requestId, OnContractDataEndMessage),
                connection.SubscribeForRequestErrors(requestId, OnError)
            };
        }

        private void SendRequest(SearchRequest request, int requestId)
        {
            var message = RequestContractDetailsMessage.Create(request);
            message.RequestId = requestId;
            connection.SendMessage(message);
        }

        private void OnContractDataMessage(ContractDataMessage message)
        {
            observer.OnNext(Contract.FromContractDataMessage(message));
        }

        private void OnContractDataEndMessage(ContractDataEndMessage message)
        {
            observer.OnCompleted();
        }

        private void OnError(Error error)
        {
            observer.OnError(new ContractSearchException(error));
        }

        [System.Diagnostics.Contracts.ContractInvariantMethod]
        private void ObjectInvariant()
        {
            CodeContract.Invariant(connection != null);
        }

        private readonly IConnection connection;
        private ICollection<IDisposable> subscriptions;
        private IObserver<Contract> observer;
    }
}
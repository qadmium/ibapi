using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Errors;
using IBApi.Exceptions;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Contracts
{
    internal sealed class FindContractOperation
    {
        private readonly List<Contract> result = new List<Contract>();

        private readonly TaskCompletionSource<IReadOnlyCollection<Contract>> taskCompletionSource =
            new TaskCompletionSource<IReadOnlyCollection<Contract>>();

        private ICollection<IDisposable> subscriptions;

        public FindContractOperation(IConnection connection, SearchRequest request,
            CancellationToken cancellationToken)
        {
            CodeContract.Requires(connection != null);

            cancellationToken.Register(() =>
            {
                this.subscriptions.Unsubscribe();
                this.taskCompletionSource.SetCanceled();
            });

            var requestId = connection.NextRequestId();
            this.Subscribe(requestId, connection);
            SendRequest(request, requestId, connection);
        }

        public Task<IReadOnlyCollection<Contract>> Task
        {
            get { return this.taskCompletionSource.Task; }
        }

        private void Subscribe(int requestId, IConnection connection)
        {
            this.subscriptions = new List<IDisposable>
            {
                connection.Subscribe((ContractDataMessage message) => message.RequestId == requestId,
                    this.OnContractDataMessage),
                connection.Subscribe((ContractDataEndMessage message) => message.RequestId == requestId,
                    this.OnContractDataEndMessage),
                connection.SubscribeForRequestErrors(requestId, this.OnError)
            };
        }

        private static void SendRequest(SearchRequest request, int requestId, IConnection connection)
        {
            var message = RequestContractDetailsMessage.Create(request);
            message.RequestId = requestId;
            connection.SendMessage(message);
        }

        private void OnContractDataMessage(ContractDataMessage message)
        {
            this.result.Add(Contract.FromContractDataMessage(message));
        }

        private void OnContractDataEndMessage(ContractDataEndMessage message)
        {
            this.taskCompletionSource.SetResult(this.result.AsReadOnly());
        }

        private void OnError(Error error)
        {
            this.taskCompletionSource.SetException(new IBException(error.Message, error.Code));
        }
    }
}
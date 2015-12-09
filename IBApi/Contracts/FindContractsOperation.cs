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
    internal sealed class FindContractsOperation
    {
        private readonly List<Contract> result = new List<Contract>();

        private readonly TaskCompletionSource<IReadOnlyCollection<Contract>> taskCompletionSource =
            new TaskCompletionSource<IReadOnlyCollection<Contract>>();

        private int resultsToRetireve;

        private ICollection<IDisposable> subscriptions;

        public FindContractsOperation(IConnection connection, IIdsDispenser dispenser, SearchRequest request,
            CancellationToken cancellationToken)
        {
            CodeContract.Requires(connection != null);
            CodeContract.Requires(dispenser != null);

            cancellationToken.Register(() =>
            {
                this.subscriptions.Unsubscribe();
                this.taskCompletionSource.SetCanceled();
            });

            this.resultsToRetireve = request.NumberOfResults ?? int.MaxValue;
            this.Subscribe(connection, dispenser, request);
        }

        public Task<IReadOnlyCollection<Contract>> Task
        {
            get { return this.taskCompletionSource.Task; }
        }

        private void Subscribe(IConnection connection, IIdsDispenser dispenser, SearchRequest request)
        {
            CodeContract.Requires(connection != null);
            CodeContract.Requires(dispenser != null);

            var requestId = dispenser.NextRequestId();
            
            this.subscriptions = new List<IDisposable>
            {
                connection.Subscribe((ContractDataMessage message) => message.RequestId == requestId,
                    this.OnContractDataMessage),
                connection.Subscribe((ContractDataEndMessage message) => message.RequestId == requestId,
                    this.OnContractDataEndMessage),
                connection.SubscribeForRequestErrors(requestId, this.OnError)
            };

            SendRequest(request, requestId, connection);
        }

        private static void SendRequest(SearchRequest request, int requestId, IConnection connection)
        {
            CodeContract.Requires(connection != null);
            var message = RequestContractDetailsMessage.Create(request);
            message.RequestId = requestId;
            connection.SendMessage(message);
        }

        private void OnContractDataMessage(ContractDataMessage message)
        {
            this.result.Add(Contract.FromContractDataMessage(message));
            this.resultsToRetireve--;
            if (this.resultsToRetireve == 0)
            {
                this.Finish();
            }
        }

        private void OnContractDataEndMessage(ContractDataEndMessage message)
        {
            this.Finish();
        }

        private void Finish()
        {
            this.subscriptions.Unsubscribe();
            this.taskCompletionSource.SetResult(this.result.AsReadOnly());
        }

        private void OnError(Error error)
        {
            this.taskCompletionSource.SetException(new IbException(error.Message, error.Code));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Executions;
using IBApi.Messages.Client;
using IBApi.Messages.Server;

namespace IBApi.Operations
{
    internal class CreateExecutionsStorageOperation
    {
        private readonly string account;
        private readonly IConnection connection;

        private readonly List<Execution> executions = new List<Execution>();
        private readonly IApiObjectsFactory factory;

        private readonly int requestId;

        private readonly TaskCompletionSource<IExecutionStorageInternal> taskCompletionSource =
            new TaskCompletionSource<IExecutionStorageInternal>();

        private CancellationToken cancellationToken;
        private List<IDisposable> subscriptions;

        public CreateExecutionsStorageOperation(IConnection connection, CancellationToken cancellationToken,
            IApiObjectsFactory factory,
            string account)
        {
            Contract.Requires(connection != null);
            Contract.Requires(factory != null);
            Contract.Requires(account != null);

            this.connection = connection;
            this.cancellationToken = cancellationToken;
            this.factory = factory;
            this.cancellationToken.Register(() =>
            {
                this.subscriptions.Unsubscribe();
                this.taskCompletionSource.SetCanceled();
            });
            this.account = account;
            this.requestId = connection.NextRequestId();

            this.Subscribe();
            this.SendRequest();
        }

        public Task<IExecutionStorageInternal> Result
        {
            get { return this.taskCompletionSource.Task; }
        }

        private void Subscribe()
        {
            this.subscriptions = new List<IDisposable>
            {
                this.connection.Subscribe((ExecutionDataMessage message) => message.RequestId == this.requestId,
                    this.OnExecutionData),
                this.connection.Subscribe((ExecutionDataEndMessage message) => message.RequestId == this.requestId,
                    this.OnExecutionDataEnd)
            };
        }

        private void SendRequest()
        {
            var request = RequestExecutionsMessage.Default;
            request.FilterByAccountCode = this.account;

            this.connection.SendMessage(request);
        }

        private void OnExecutionDataEnd(ExecutionDataEndMessage message)
        {
            this.subscriptions.Unsubscribe();

            if (this.cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.taskCompletionSource.SetResult(this.factory.CreateExecutionStorage(this.account, this.executions));
        }

        private void OnExecutionData(ExecutionDataMessage message)
        {
            this.executions.Add(Execution.FromMessage(message, this.account));
        }
    }
}
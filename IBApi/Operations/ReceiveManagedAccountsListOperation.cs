using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Messages.Server;

namespace IBApi.Operations
{
    internal sealed class ReceiveManagedAccountsListOperation
    {
        private readonly IDisposable subscription;
        private readonly TaskCompletionSource<string[]> taskCompletionSource = new TaskCompletionSource<string[]>();

        private CancellationToken cancellationToken;

        public ReceiveManagedAccountsListOperation(IConnection connection, CancellationToken cancellationToken)
        {
            Contract.Requires(!cancellationToken.IsCancellationRequested);
            this.cancellationToken = cancellationToken;
            this.cancellationToken.Register(() =>
            {
                this.subscription.Dispose();
                this.taskCompletionSource.SetCanceled();
            });
            this.subscription = connection.Subscribe<ManagedAccountsListMessage>(this.OnManagedAccountList);
        }

        public Task<string[]> Result
        {
            get { return this.taskCompletionSource.Task; }
        }

        private void OnManagedAccountList(ManagedAccountsListMessage message)
        {
            this.subscription.Dispose();

            if (this.cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Trace.TraceInformation("Received accouns list");
            this.taskCompletionSource.SetResult(message.AccountsList.Split(new[] {','},
                StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
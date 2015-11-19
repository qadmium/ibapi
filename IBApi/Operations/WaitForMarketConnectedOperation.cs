using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Errors;
using IBApi.Exceptions;

namespace IBApi.Operations
{
    internal sealed class WaitForMarketConnectedOperation
    {
        public WaitForMarketConnectedOperation(IConnection connection, CancellationToken cancellationToken)
        {
            Contract.Requires(connection != null);
            Contract.Requires(!cancellationToken.IsCancellationRequested);

            this.cancellationToken = cancellationToken;
            this.cancellationToken.Register(() =>
            {
                this.subscriptions.Unsubscribe();
                this.taskCompletionSource.SetCanceled();
            });
            this.subscriptions = new List<IDisposable>
            {
                connection.SubscribeForErrors(error => error.Code.IsConnected(), this.OnMarketConnected),
                connection.SubscribeForErrors(error => error.Code.IsGeneralError(), this.OnMarketNotConnected)
            };
        }

        private readonly ICollection<IDisposable> subscriptions;
        private CancellationToken cancellationToken;
        private readonly TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

        public Task Result
        {
            get { return this.taskCompletionSource.Task; }
        }
        private void OnMarketConnected(Error error)
        {
            this.subscriptions.Unsubscribe();

            if (this.cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Trace.TraceInformation(error.Message);
            this.taskCompletionSource.SetResult(true);
        }
        private void OnMarketNotConnected(Error error)
        {
            this.subscriptions.Unsubscribe();

            if (this.cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Trace.TraceInformation(error.Message);
            this.taskCompletionSource.SetException(new IBException(error.Message, error.Code));
        }
    }
}
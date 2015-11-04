using System;
using System.Collections.Generic;
using System.Diagnostics;
using IBApi.Connection;
using IBApi.Errors;

namespace IBApi.Operations
{
    sealed class WaitForMarketConnectedOperation : AbstractOperation
    {
        public override void Execute(IConnection connection)
        {
            subscriptions = new List<IDisposable>
            {
                connection.SubscribeForErrors(error => error.Code.IsConnected(), OnMarketConnected),
                connection.SubscribeForErrors(error => error.Code.IsGeneralError(), OnMarketNotConnected)
            };
        }

        public override void Dispose()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            subscriptions.Unsubscribe();
        }

        private void OnMarketConnected(Error error)
        {
            Trace.TraceInformation(error.Message);
            OnOperationCompleted();
            Unsubscribe();
        }

        private void OnMarketNotConnected(Error error)
        {
            Trace.TraceInformation(error.Message);
            OnOperationFailed(error);
            Unsubscribe();
        }

        private ICollection<IDisposable> subscriptions;
    }
}

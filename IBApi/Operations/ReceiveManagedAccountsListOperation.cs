using System;
using System.Diagnostics;
using IBApi.Connection;
using IBApi.Messages.Server;

namespace IBApi.Operations
{
    sealed class ReceiveManagedAccountsListOperation : AbstractOperation, IReceiveManagedAccountsListOperation
    {
        public override void Execute(IConnection connection)
        {
            subscription = connection.Subscribe<ManagedAccountsListMessage>(OnManagedAccountList);
        }

        public string[] AccountsList { get; private set; }

        public override void Dispose()
        {
            subscription.Dispose();
        }

        private void OnManagedAccountList(ManagedAccountsListMessage message)
        {
            Trace.TraceInformation("Received accouns list");
            AccountsList = message.AccountsList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            OnOperationCompleted();
        }

        private IDisposable subscription;
    }
}

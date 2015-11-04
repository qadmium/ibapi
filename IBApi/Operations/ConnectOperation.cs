using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using IBApi.Accounts;
using IBApi.Connection;
using IBApi.Errors;

namespace IBApi.Operations
{
    internal sealed class ConnectOperation : IDisposable
    {
        public ConnectOperation(IApiObjectsFactory objectsFactory, IConnection connection, Action<IClient> onSucessCallback, Action<Error> onErrorCallback)
        {
            Contract.Requires<ArgumentNullException>(connection != null);
            Contract.Requires<ArgumentNullException>(onSucessCallback != null);
            Contract.Requires<ArgumentNullException>(onErrorCallback != null);

            this.onSucessCallback = onSucessCallback;
            this.onErrorCallback = onErrorCallback;
            this.objectsFactory = objectsFactory;
            this.connection = connection;

            waitForMarketConnectedOperation = this.objectsFactory.CreateWaitForMarketConnectedOperation();
            receiveManagedAccountsListOperation = this.objectsFactory.CreateReceiveManagedAccountsListOperation();
        }

        public void Execute()
        {
            waitForMarketConnectedOperation.OperationCompleted += OnWaitForMarketConnectedOperationCompleted;
            waitForMarketConnectedOperation.OperationFailed += OnWaitForMarketConnectedOperationFailed;
            waitForMarketConnectedOperation.Execute(connection);

            receiveManagedAccountsListOperation.OperationCompleted += OnReceiveManagedAccountsListOperationCompleted;
            receiveManagedAccountsListOperation.Execute(connection);

            connection.Run();
        }

        private void OnWaitForMarketConnectedOperationFailed(Error error)
        {
            onErrorCallback(error);
            Clear();
        }

        private void OnWaitForMarketConnectedOperationCompleted()
        {
            CheckAllConditionsSatisfied();
        }

        private void OnReceiveManagedAccountsListOperationCompleted()
        {
            accountsStorage =
                objectsFactory.CreateAccountStorage(receiveManagedAccountsListOperation.AccountsList);

            accountsStorage.Initialized += OnAccountsStorageInitialized;
        }

        private void OnAccountsStorageInitialized()
        {
            CheckAllConditionsSatisfied();
        }

        public void Dispose()
        {
            Clear();
        }

        private void Clear()
        {
            if (receiveManagedAccountsListOperation != null)
            {
                receiveManagedAccountsListOperation.Dispose();
            }

            if (accountsStorage != null)
            {
                accountsStorage.Dispose();
            }

            if (waitForMarketConnectedOperation != null)
            {
                waitForMarketConnectedOperation.Dispose();
            }

            connection.Dispose();
        }

        private void CheckAllConditionsSatisfied()
        {
            if (!AccountStorageInited() || !MarketConnected())
            {
                return;
            }

            Trace.TraceInformation("Connected");

            var client = objectsFactory.CreateClient(accountsStorage);
            onSucessCallback(client);
        }

        private bool MarketConnected()
        {
            return waitForMarketConnectedOperation.Completed;
        }

        private bool AccountStorageInited()
        {
            return accountsStorage != null && accountsStorage.IsInitialized;
        }

        private readonly IConnection connection;
        private readonly Action<IClient> onSucessCallback;
        private readonly Action<Error> onErrorCallback;
        private IAccountsStorage accountsStorage;

        private readonly IOperation waitForMarketConnectedOperation;
        private readonly IReceiveManagedAccountsListOperation receiveManagedAccountsListOperation;
        private readonly IApiObjectsFactory objectsFactory;

    }
}
using System;
using IBApi.Accounts;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Executions;
using IBApi.MarketDepth;
using IBApi.Operations;
using IBApi.Orders;
using IBApi.Positions;
using IBApi.Quotes;

namespace IBApi
{
    sealed class ApiObjectsFactory : IApiObjectsFactory
    {
        public ApiObjectsFactory(IConnection connection)
        {
            this.connection = connection;
        }

        public IReceiveManagedAccountsListOperation CreateReceiveManagedAccountsListOperation()
        {
            return new ReceiveManagedAccountsListOperation();
        }

        public IAccountsStorage CreateAccountStorage(string[] managedAccountsList)
        {
            return new AccountsStorage(managedAccountsList, connection, this);
        }

        public IAccountInternal CreateAccount(string accountName)
        {
            return new Account(accountName, connection, this);
        }

        public IOperation CreateWaitForMarketConnectedOperation()
        {
            return new WaitForMarketConnectedOperation();
        }

        public IClient CreateClient(IAccountsStorage accountsStorage)
        {
            return new Client(this, connection, accountsStorage);
        }

        public SyncFindContractOperation CreateSyncFindContractOperation()
        {
            return new SyncFindContractOperation(connection);
        }

        public AsyncFindContractOperation CreateAsyncFindContractOperation()
        {
            return new AsyncFindContractOperation(connection);
        }

        public IDisposable CreateQuoteSubscription(IQuoteObserver observer, Contract contract)
        {
            return new QuoteSubscription(connection, observer, contract);
        }

        public IDisposable CreateMarketDepthSubscription(IMarketDepthObserver observer, Contract contract)
        {
            return new MarketDepthSubscription(connection, observer, contract);
        }

        public IExecutionStorageInternal CreateExecutionStorage(string accountName)
        {
            return new ExecutionsStorage(connection, accountName);
        }

        public IPositionsStorageInternal CreatePositionStorage(string accountName)
        {
            return new PositionsStorage(connection, this, accountName);
        }

        public IOrdersStorageInternal CreateOrdersStorage(string accountName)
        {
            return new OrdersStorage(connection, this, accountName);
        }

        public Position CreatePosition()
        {
            return new Position();
        }

        public Order CreateOrder(int orderId)
        {
            return new Order(orderId, connection);
        }

        private readonly IConnection connection;
    }
}
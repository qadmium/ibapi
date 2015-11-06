using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    internal sealed class ApiObjectsFactory : IApiObjectsFactory
    {
        private readonly IConnection connection;

        public ApiObjectsFactory(IConnection connection)
        {
            this.connection = connection;
        }

        public Task<string[]> CreateReceiveManagedAccountsListOperation(CancellationToken cancellationToken)
        {
            return new ReceiveManagedAccountsListOperation(this.connection, cancellationToken).Result;
        }

        public Task<IAccountsStorage> CreateAccountStorageOperation(string[] managedAccountsList,
            CancellationToken cancellationToken)
        {
            return
                new CreateAccountStorageOperation(cancellationToken, this, this.connection, managedAccountsList).Result;
        }

        public IAccountsStorage CreateAccountStorage(List<IAccountInternal> accounts)
        {
            return new AccountsStorage(accounts);
        }

        public Task<IAccountInternal> CreateAccountOperation(string account, CancellationToken cancellationToken)
        {
            return new CreateAccountOperation(this.connection, this, account, cancellationToken).Result;
        }

        public IAccountInternal CreateAccount(string accountName, IExecutionStorageInternal executionStorage,
            IPositionsStorageInternal positionsStorage, IOrdersStorageInternal ordersStorage,
            AccountCurrenciesFields accountCurrenciesFields)
        {
            return new Account(accountName, this.connection, executionStorage, positionsStorage, ordersStorage,
                accountCurrenciesFields);
        }

        public Task CreateWaitForMarketConnectedOperation(CancellationToken cancellationToken)
        {
            return new WaitForMarketConnectedOperation(this.connection, cancellationToken).Result;
        }

        public IClient CreateClient(IAccountsStorage accountsStorage)
        {
            return new Client(this, this.connection, accountsStorage);
        }

        public Task<IReadOnlyCollection<Contract>> CreateAsyncFindContractOperation(SearchRequest searchRequest,
            CancellationToken cancellationToken)
        {
            return new AsyncFindContractOperation(this.connection, searchRequest, cancellationToken).Task;
        }

        public IDisposable CreateQuoteSubscription(IQuoteObserver observer, Contract contract)
        {
            return new QuoteSubscription(this.connection, observer, contract);
        }

        public IDisposable CreateMarketDepthSubscription(IMarketDepthObserver observer, Contract contract)
        {
            return new MarketDepthSubscription(this.connection, observer, contract);
        }

        public Task<IExecutionStorageInternal> CreateExecutionStorageOperation(string accountName,
            CancellationToken cancellationToken)
        {
            return new CreateExecutionsStorageOperation(this.connection, cancellationToken, this, accountName).Result;
        }

        public IExecutionStorageInternal CreateExecutionStorage(string accountName, List<Execution> executions)
        {
            return new ExecutionsStorage(this.connection, accountName, executions);
        }

        public IPositionsStorageInternal CreatePositionStorage(string accountName)
        {
            return new PositionsStorage(this.connection, this, accountName);
        }

        public IOrdersStorageInternal CreateOrdersStorage(string accountName)
        {
            return new OrdersStorage(this.connection, this, accountName);
        }

        public Position CreatePosition()
        {
            return new Position();
        }

        public Order CreateOrder(int orderId)
        {
            return new Order(orderId, this.connection);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Accounts;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Executions;
using IBApi.Infrastructure;
using IBApi.MarketDepth;
using IBApi.Messages.Client;
using IBApi.Operations;
using IBApi.Orders;
using IBApi.Positions;
using IBApi.Quotes;

namespace IBApi
{
    internal sealed class ApiObjectsFactory : IApiObjectsFactory
    {
        private readonly IConnection connection;
        private readonly IIdsDispenser idsDispenser;
        private readonly CancellationTokenSource internalCancellationTokenSource;
        private readonly ProxiesFactory proxiesFactory;

        public ApiObjectsFactory(IConnection connection, IIdsDispenser idsDispenser, Dispatcher dispatcher,
            CancellationTokenSource internalCancellationTokenSource)
        {
            System.Diagnostics.Contracts.Contract.Requires(connection != null);
            System.Diagnostics.Contracts.Contract.Requires(idsDispenser != null);

            this.connection = connection;
            this.idsDispenser = idsDispenser;
            this.internalCancellationTokenSource = internalCancellationTokenSource;
            this.proxiesFactory = new ProxiesFactory(dispatcher);
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
            return new AccountsStorage(accounts, this.proxiesFactory);
        }

        public Task<IAccountInternal> CreateAccountOperation(string account, CancellationToken cancellationToken)
        {
            return new CreateAccountOperation(this.connection, this, account, cancellationToken).Result;
        }

        public IAccountInternal CreateAccount(string accountName, IExecutionStorageInternal executionStorage,
            IPositionsStorageInternal positionsStorage, IOrdersStorageInternal ordersStorage,
            AccountCurrenciesFields accountCurrenciesFields)
        {
            return new Account(accountName, this.connection, this, executionStorage, positionsStorage, ordersStorage,
                this.idsDispenser, this.internalCancellationTokenSource,
                accountCurrenciesFields);
        }

        public Task CreateWaitForMarketConnectedOperation(CancellationToken cancellationToken)
        {
            return new WaitForMarketConnectedOperation(this.connection, cancellationToken).Result;
        }

        public IClient CreateClient(IAccountsStorage accountsStorage)
        {
            return new Client(this, accountsStorage, this.internalCancellationTokenSource);
        }

        public Task<IReadOnlyCollection<Contract>> CreateAsyncFindContractOperation(SearchRequest searchRequest,
            CancellationToken cancellationToken)
        {
            return new FindContractsOperation(this.connection, this.idsDispenser, searchRequest, cancellationToken).Task;
        }

        public IDisposable CreateQuoteSubscription(IQuoteObserver observer, Contract contract)
        {
            return new QuoteSubscription(this.connection, this.idsDispenser, observer, contract);
        }

        public IDisposable CreateMarketDepthSubscription(IMarketDepthObserver observer, Contract contract)
        {
            return new MarketDepthSubscription(this.connection, this.idsDispenser, observer, contract);
        }

        public Task<IExecutionStorageInternal> CreateExecutionStorageOperation(string accountName,
            CancellationToken cancellationToken)
        {
            return
                new CreateExecutionsStorageOperation(this.connection, this.idsDispenser, cancellationToken, this,
                    accountName).Result;
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

        public Order CreateOrder(int orderId, string account)
        {
            return new Order(orderId, account, this.connection, this, this.internalCancellationTokenSource);
        }

        public Task<int> CreatePlaceOrderOperation(RequestPlaceOrderMessage requestPlaceOrderMessage,
            IOrdersStorageInternal ordersStorage,
            CancellationToken cancellationToken)
        {
            return
                new PlaceOrderOperation(requestPlaceOrderMessage, this.connection, ordersStorage, cancellationToken)
                    .Result;
        }

        public Task CreateWaitForOrderFillOperation(IOrder order, CancellationToken cancellationToken)
        {
            return new WaitForOrderFillOperation(order, cancellationToken).Result;
        }

        public void Dispose()
        {
            this.internalCancellationTokenSource.Cancel();
            this.idsDispenser.Dispose();
            this.connection.Dispose();
        }
    }
}
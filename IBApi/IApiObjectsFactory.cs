using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Accounts;
using IBApi.Contracts;
using IBApi.Executions;
using IBApi.Orders;
using IBApi.Positions;

namespace IBApi
{
    internal interface IApiObjectsFactory
    {
        Task<string[]> CreateReceiveManagedAccountsListOperation(CancellationToken cancellationToken);
        Task<IAccountsStorage> CreateAccountStorageOperation(string[] managedAccountsList, CancellationToken cancellationToken);
        IAccountsStorage CreateAccountStorage(List<IAccountInternal> accounts);
        Task<IAccountInternal> CreateAccountOperation(string account, CancellationToken cancellationToken);
        IAccountInternal CreateAccount(string accountName, IExecutionStorageInternal executionStorage,
            IPositionsStorageInternal positionsStorage, IOrdersStorageInternal ordersStorage,
            AccountCurrenciesFields accountCurrenciesFields);
        Task CreateWaitForMarketConnectedOperation(CancellationToken cancellationToken);
        IClient CreateClient(IAccountsStorage accountsStorage);
        Task<IReadOnlyCollection<Contract>> CreateAsyncFindContractOperation(SearchRequest searchRequest, CancellationToken cancellationToken);
        IDisposable CreateQuoteSubscription(IQuoteObserver observer, Contract contract);
        IDisposable CreateMarketDepthSubscription(IMarketDepthObserver observer, Contract contract);
        Task<IExecutionStorageInternal> CreateExecutionStorageOperation(string accountName, CancellationToken cancellationToken);
        IExecutionStorageInternal CreateExecutionStorage(string accountName, List<Execution> executions);
        IPositionsStorageInternal CreatePositionStorage(string accountName);
        IOrdersStorageInternal CreateOrdersStorage(string accountName);
        Position CreatePosition();
        Order CreateOrder(int orderId);
    }
}
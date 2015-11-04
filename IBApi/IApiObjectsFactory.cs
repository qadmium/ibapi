using System;
using IBApi.Accounts;
using IBApi.Contracts;
using IBApi.Executions;
using IBApi.Messages.Server;
using IBApi.Operations;
using IBApi.Orders;
using IBApi.Positions;

namespace IBApi
{
    internal interface IApiObjectsFactory
    {
        IReceiveManagedAccountsListOperation CreateReceiveManagedAccountsListOperation();
        IAccountsStorage CreateAccountStorage(string[] managedAccountsList);
        IAccountInternal CreateAccount(string accountName);
        IOperation CreateWaitForMarketConnectedOperation();
        IClient CreateClient(IAccountsStorage accountsStorage);
        SyncFindContractOperation CreateSyncFindContractOperation();
        AsyncFindContractOperation CreateAsyncFindContractOperation();
        IDisposable CreateQuoteSubscription(IQuoteObserver observer, Contract contract);
        IDisposable CreateMarketDepthSubscription(IMarketDepthObserver observer, Contract contract);
        IExecutionStorageInternal CreateExecutionStorage(string accountName);
        IPositionsStorageInternal CreatePositionStorage(string accountName);
        IOrdersStorageInternal CreateOrdersStorage(string accountName);
        Position CreatePosition();
        Order CreateOrder(int orderId);
    }
}
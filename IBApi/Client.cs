using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using IBApi.Accounts;
using IBApi.Connection;
using IBApi.Contracts;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi
{
    internal sealed class Client : IClient
    {
        public Client(IApiObjectsFactory objectsFactory, IConnection connection, IAccountsStorage accountsStorage)
        {
            CodeContract.Requires(objectsFactory != null);
            CodeContract.Requires(connection != null);
            CodeContract.Requires(accountsStorage != null);
            CodeContract.Requires(accountsStorage.Accounts.Count() != 0);

            this.objectsFactory = objectsFactory;
            this.connection = connection;
            this.accountsStorage = accountsStorage;
        }

        public ReadOnlyCollection<IAccount> Accounts { get { return accountsStorage.Accounts; } }
        
        public Contract FindFirstContract(SearchRequest request, int millisecondsTimeout)
        {
            var findContractOperation = objectsFactory.CreateSyncFindContractOperation();
            var result = findContractOperation.ResultFor(request, millisecondsTimeout);
            findContractOperation.Dispose();
            return result;
        }

        public IDisposable FindContracts(IObserver<Contract> contractsObserver, SearchRequest request)
        {
            var findContractOperation = objectsFactory.CreateAsyncFindContractOperation();
            findContractOperation.Start(contractsObserver, request);
            return findContractOperation;
        }

        public IDisposable SubscribeQuote(IQuoteObserver observer, Contract contract)
        {
            return objectsFactory.CreateQuoteSubscription(observer, contract);
        }

        public IDisposable SubscribeMarketDepth(IMarketDepthObserver observer, Contract contract)
        {
            return objectsFactory.CreateMarketDepthSubscription(observer, contract);
        }

        public void Dispose()
        {
            Trace.TraceInformation("Disposing client");
            connection.Dispose();
        }

        private readonly IAccountsStorage accountsStorage;
        private readonly IConnection connection;
        private readonly IApiObjectsFactory objectsFactory;
    }
}

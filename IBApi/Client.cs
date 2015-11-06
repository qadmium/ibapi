using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Accounts;
using IBApi.Connection;
using IBApi.Contracts;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi
{
    internal sealed class Client : IClient
    {
        private readonly IAccountsStorage accountsStorage;
        private readonly IConnection connection;
        private readonly IApiObjectsFactory objectsFactory;

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

        public IReadOnlyCollection<IAccount> Accounts
        {
            get { return this.accountsStorage.Accounts; }
        }

        public Task<IReadOnlyCollection<Contract>> FindContracts(SearchRequest request, CancellationToken cancellationToken)
        {
            return this.objectsFactory.CreateAsyncFindContractOperation(request, cancellationToken);
        }

        public IDisposable SubscribeQuote(IQuoteObserver observer, Contract contract)
        {
            return this.objectsFactory.CreateQuoteSubscription(observer, contract);
        }

        public IDisposable SubscribeMarketDepth(IMarketDepthObserver observer, Contract contract)
        {
            return this.objectsFactory.CreateMarketDepthSubscription(observer, contract);
        }

        public void Dispose()
        {
            Trace.TraceInformation("Disposing client");
            this.connection.Dispose();
        }
    }
}
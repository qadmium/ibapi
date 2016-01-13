using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Accounts;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Infrastructure;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi
{
    internal sealed class Client : IClient
    {
        private readonly object accountChangedEvent;
        private readonly IAccountsStorage accountsStorage;
        private readonly Dispatcher dispatcher;
        private readonly CancellationTokenSource internalCancelationTokenSource;
        private readonly IApiObjectsFactory objectsFactory;

        public Client(IApiObjectsFactory objectsFactory, IAccountsStorage accountsStorage, Dispatcher dispatcher,
            IConnection connection,
            CancellationTokenSource internalCancelationTokenSource)
        {
            CodeContract.Requires(objectsFactory != null);
            CodeContract.Requires(accountsStorage != null);
            CodeContract.Requires(dispatcher != null);
            CodeContract.Requires(connection != null);
            CodeContract.Requires(accountsStorage.Accounts.Count() != 0);

            this.objectsFactory = objectsFactory;
            this.accountsStorage = accountsStorage;
            this.dispatcher = dispatcher;
            this.internalCancelationTokenSource = internalCancelationTokenSource;
            this.accountChangedEvent = dispatcher.RegisterEvent();
            connection.OnDisconnect += this.ConnectionOnOnDisconnect;
        }

        private void ConnectionOnOnDisconnect(object sender, DisconnectedEventArgs disconnectedEventArgs)
        {
            this.dispatcher.RaiseEvent(this.accountChangedEvent, this, disconnectedEventArgs);
            this.Dispose();
        }

        public IReadOnlyCollection<IAccount> Accounts
        {
            get { return this.accountsStorage.Accounts; }
        }

        public Task<IReadOnlyCollection<Contract>> FindContracts(SearchRequest request,
            CancellationToken cancellationToken)
        {
            using (
                var cancellationTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(this.internalCancelationTokenSource.Token,
                        cancellationToken))
            {
                return this.objectsFactory.CreateAsyncFindContractOperation(request, cancellationTokenSource.Token);
            }
        }

        public IDisposable SubscribeQuote(IQuoteObserver observer, Contract contract)
        {
            return this.objectsFactory.CreateQuoteSubscription(observer, contract);
        }

        public IDisposable SubscribeMarketDepth(IMarketDepthObserver observer, Contract contract)
        {
            return this.objectsFactory.CreateMarketDepthSubscription(observer, contract);
        }

        public event EventHandler<DisconnectedEventArgs> ConnectionDisconnected
        {
            add { this.dispatcher.AddHandler(this.accountChangedEvent, value); }
            remove { this.dispatcher.RemoveHandler(this.accountChangedEvent, value); }
        }

        public void Dispose()
        {
            Trace.TraceInformation("Disposing client");
            this.objectsFactory.Dispose();
        }
    }
}
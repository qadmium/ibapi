using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Accounts;
using IBApi.Contracts;
using CodeContract = System.Diagnostics.Contracts.Contract;
using ContractClassFor = System.Diagnostics.Contracts.ContractClassForAttribute;

namespace IBApi
{
    [ContractClassFor(typeof (IClient))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IClientContract : IClient
    {
        public void Dispose()
        {
        }

        public IReadOnlyCollection<IAccount> Accounts
        {
            get
            {
                CodeContract.Ensures(CodeContract.Result<IReadOnlyCollection<IAccount>>() != null);
                return null;
            }
        }

        public abstract Task<IReadOnlyCollection<Contract>> FindContracts(SearchRequest request,
            CancellationToken cancellationToken);

        public IDisposable SubscribeQuote(IQuoteObserver observer, Contract contract)
        {
            CodeContract.Requires(observer != null);
            CodeContract.Ensures(CodeContract.Result<IDisposable>() != null);
            return null;
        }

        public IDisposable SubscribeMarketDepth(IMarketDepthObserver observer, Contract contract)
        {
            CodeContract.Requires(observer != null);
            CodeContract.Ensures(CodeContract.Result<IDisposable>() != null);
            return null;
        }

        public abstract event EventHandler<DisconnectedEventArgs> ConnectionDisconnected;
    }
}
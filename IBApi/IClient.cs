using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Accounts;
using IBApi.Contracts;
using ContractClass = System.Diagnostics.Contracts.ContractClassAttribute;

namespace IBApi
{
    [ContractClass(typeof(IClientContract))]
    public interface IClient : IDisposable
    {
        IReadOnlyCollection<IAccount> Accounts { get; }

        Task<IReadOnlyCollection<Contract>> FindContracts(SearchRequest request, CancellationToken cancellationToken);
        IDisposable SubscribeQuote(IQuoteObserver observer, Contract contract);
        IDisposable SubscribeMarketDepth(IMarketDepthObserver observer, Contract contract);
    }
}
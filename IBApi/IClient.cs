using System;
using System.Collections.ObjectModel;
using IBApi.Accounts;
using IBApi.Contracts;
using ContractClass = System.Diagnostics.Contracts.ContractClassAttribute;

namespace IBApi
{
    [ContractClass(typeof(IClientContract))]
    public interface IClient : IDisposable
    {
        ReadOnlyCollection<IAccount> Accounts { get; }

        Contract FindFirstContract(SearchRequest request, int millisecondsTimeout);
        IDisposable FindContracts(IObserver<Contract> contractsObserver, SearchRequest request);
        IDisposable SubscribeQuote(IQuoteObserver observer, Contract contract);
        IDisposable SubscribeMarketDepth(IMarketDepthObserver observer, Contract contract);
    }
}
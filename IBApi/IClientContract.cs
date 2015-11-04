using System;
using System.Collections.ObjectModel;
using IBApi.Accounts;
using IBApi.Contracts;
using CodeContract = System.Diagnostics.Contracts.Contract;
using ContractClassFor = System.Diagnostics.Contracts.ContractClassForAttribute;

namespace IBApi
{
    [ContractClassFor(typeof(IClient))]
    internal abstract class IClientContract : IClient
    {
        public void Dispose()
        {
        }

        public ReadOnlyCollection<IAccount> Accounts
        {
            get
            {
                CodeContract.Ensures(CodeContract.Result<ReadOnlyCollection<IAccount>>() != null);
                return null;
            }
        }

        public Contract FindFirstContract(SearchRequest request, int millisecondsTimeout)
        {
            return default(Contract);
        }

        public IDisposable FindContracts(IObserver<Contract> contractsObserver, SearchRequest request)
        {
            CodeContract.Requires<ArgumentNullException>(contractsObserver != null);
            CodeContract.Ensures(CodeContract.Result<IDisposable>() != null);
            return null;
        }

        public IDisposable SubscribeQuote(IQuoteObserver observer, Contract contract)
        {
            CodeContract.Requires<ArgumentNullException>(observer != null);
            CodeContract.Ensures(CodeContract.Result<IDisposable>() != null);
            return null;
        }

        public IDisposable SubscribeMarketDepth(IMarketDepthObserver observer, Contract contract)
        {
            CodeContract.Requires<ArgumentNullException>(observer != null);
            CodeContract.Ensures(CodeContract.Result<IDisposable>() != null);
            return null;
        }
    }
}
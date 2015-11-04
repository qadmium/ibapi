using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Accounts
{
    [ContractClassFor(typeof(IAccountsStorage))]
    internal abstract class IAccountStorageContract : IAccountsStorage
    {
        public abstract event InitializedEventHandler Initialized;
        public bool IsInitialized { get { return false; } }

        public ReadOnlyCollection<IAccount> Accounts
        {
            get
            {
                Contract.Requires<InvalidOperationException>(IsInitialized);
                Contract.Ensures(Contract.Result<ReadOnlyCollection<IAccount>>() != null);
                return null;
            }
        }

        public void Dispose()
        {
        }
    }
}

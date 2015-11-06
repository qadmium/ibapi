using System.Collections.Generic;
using System.Diagnostics.Contracts;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Accounts
{
    [ContractClassFor(typeof(IAccountsStorage))]
    // ReSharper disable once InconsistentNaming
    internal abstract class IAccountStorageContract : IAccountsStorage
    {
        public IReadOnlyCollection<IAccount> Accounts
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IAccount>>() != null);
                return null;
            }
        }

        public void Dispose()
        {
        }
    }
}

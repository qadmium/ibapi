using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace IBApi.Accounts
{
    [ContractClass(typeof(IAccountStorageContract))]
    internal interface IAccountsStorage : IDisposable
    {
        IReadOnlyCollection<IAccount> Accounts { get; }
    }
}
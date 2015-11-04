using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace IBApi.Accounts
{
    [ContractClass(typeof(IAccountStorageContract))]
    internal interface IAccountsStorage : IDisposable
    {
        event InitializedEventHandler Initialized;
        bool IsInitialized { get; }

        ReadOnlyCollection<IAccount> Accounts { get; }
    }
}
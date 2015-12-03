using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using IBApi.Infrastructure;

namespace IBApi.Accounts
{
    internal sealed class AccountsStorage : IAccountsStorage
    {
        private readonly ReadOnlyCollection<IAccountInternal> accounts;
        private readonly ReadOnlyCollection<IAccount> accountsProxies;

        public AccountsStorage(List<IAccountInternal> accounts, ProxiesFactory proxiesFactory)
        {
            Contract.Requires(accounts != null);
            Contract.Requires(proxiesFactory != null);

            this.accounts = accounts.AsReadOnly();
            this.accountsProxies = this.accounts.Select(proxiesFactory.CreateAccountProxy).ToList().AsReadOnly();
        }

        public IReadOnlyCollection<IAccount> Accounts
        {
            get { return this.accountsProxies; }
        }

        public void Dispose()
        {
            foreach (var account in this.accounts)
            {
                account.Dispose();
            }
        }
    }
}
using System.Collections.Generic;

namespace IBApi.Accounts
{
    internal sealed class AccountsStorage : IAccountsStorage
    {
        private readonly List<IAccountInternal> accounts;

        public AccountsStorage(List<IAccountInternal> accounts)
        {
            this.accounts = accounts;
        }

        public IReadOnlyCollection<IAccount> Accounts
        {
            get { return this.accounts.AsReadOnly(); }
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
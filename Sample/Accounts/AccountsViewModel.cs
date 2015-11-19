using Caliburn.Micro;
using IBApi;

namespace Sample.Accounts
{
    internal class AccountsViewModel : Screen
    {
        private IObservableCollection<Account> accounts;

        public AccountsViewModel(IClient client)
        {
            this.DisplayName = "Accounts";

            this.Accounts = new BindableCollection<Account>();

            foreach (var account in client.Accounts)
            {
                this.Accounts.Add(new Account(account));
            }
        }

        public IObservableCollection<Account> Accounts
        {
            get { return this.accounts; }
            set
            {
                if (value == this.accounts) return;
                this.accounts = value;
                this.NotifyOfPropertyChange(() => this.Accounts);
            }
        }
    }
}
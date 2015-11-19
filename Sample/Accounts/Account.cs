using Caliburn.Micro;
using IBApi.Accounts;

namespace Sample.Accounts
{
    internal class Account : PropertyChangedBase
    {
        private string accountId;
        private double stockMarketValue;
        private double totalCashBalance;
        private double unrealizedPnL;

        public Account(IAccount account)
        {
            account.AccountChanged += this.OnAccountChanged;
            this.AccountId = account.AccountId;
            this.OnAccountChanged(account);
        }

        public string AccountId
        {
            get { return this.accountId; }
            set
            {
                if (value == this.accountId) return;
                this.accountId = value;
                this.NotifyOfPropertyChange(() => this.AccountId);
            }
        }

        public double StockMarketValue
        {
            get { return this.stockMarketValue; }
            set
            {
                if (value.Equals(this.stockMarketValue)) return;
                this.stockMarketValue = value;
                this.NotifyOfPropertyChange(() => this.StockMarketValue);
            }
        }

        public double TotalCashBalance
        {
            get { return this.totalCashBalance; }
            set
            {
                if (value.Equals(this.totalCashBalance)) return;
                this.totalCashBalance = value;
                this.NotifyOfPropertyChange(() => this.TotalCashBalance);
            }
        }

        public double UnrealizedPnL
        {
            get { return this.unrealizedPnL; }
            set
            {
                if (value.Equals(this.unrealizedPnL)) return;
                this.unrealizedPnL = value;
                this.NotifyOfPropertyChange(() => this.UnrealizedPnL);
            }
        }

        private void OnAccountChanged(IAccount account)
        {
            this.TotalCashBalance = account.AccountFields.TotalCashBalance;
            this.StockMarketValue = account.AccountFields.StockMarketValue;
            this.UnrealizedPnL = account.AccountFields.UnrealizedPnL;
        }
    }
}
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Caliburn.Micro;
using IBApi;
using IBApi.Contracts;
using IBApi.Exceptions;
using IBApi.Orders;

namespace Sample.PlaceOrder
{
    internal sealed class PlaceOrderViewModel : Screen, INotifyDataErrorInfo
    {
        private readonly IClient client;
        private int quantity;
        private string ticker;
        private CancellationTokenSource cancellationTokenSource;
        private IObservableCollection<string> accounts;
        private string selectedAccount;
        private bool controlsEnabled;

        public PlaceOrderViewModel(IClient client)
        {
            this.DisplayName = "Place market order";
            this.client = client;

            this.Accounts = new BindableCollection<string>();
            this.Accounts.AddRange(client.Accounts.Select(account => account.AccountId));
            this.SelectedAccount = client.Accounts.Select(account => account.AccountId).First();
            this.ControlsEnabled = true;

            this.PropertyChanged += this.OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Ticker")
            {
                this.HasErrors = false;
                this.ErrorsChanged(this, new DataErrorsChangedEventArgs("Ticker"));
            }
        }

        public IObservableCollection<string> Accounts
        {
            get { return this.accounts; }
            set
            {
                if (Equals(value, this.accounts)) return;
                this.accounts = value;
                this.NotifyOfPropertyChange(() => this.Accounts);
            }
        }

        public string SelectedAccount
        {
            get { return this.selectedAccount; }
            set
            {
                if (value == this.selectedAccount) return;
                this.selectedAccount = value;
                this.NotifyOfPropertyChange(() => this.SelectedAccount);
            }
        }

        public int Quantity
        {
            get { return this.quantity; }
            set
            {
                if (value == this.quantity) return;
                this.quantity = value;
                this.NotifyOfPropertyChange(() => this.Quantity);
            }
        }

        public string Ticker
        {
            get { return this.ticker; }
            set
            {
                if (value == this.ticker) return;
                this.ticker = value;
                this.NotifyOfPropertyChange(() => this.Ticker);
            }
        }

        public bool ControlsEnabled
        {
            get { return this.controlsEnabled; }
            set
            {
                if (value == this.controlsEnabled) return;
                this.controlsEnabled = value;
                this.NotifyOfPropertyChange(() => this.ControlsEnabled);
            }
        }

        public void Sell()
        {
            
            this.PlaceOrder(OrderAction.Sell);
        }

        public void Buy()
        {
            this.PlaceOrder(OrderAction.Buy);
        }

        public void Cancel()
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
            }
            this.TryClose();
        }

        private async void PlaceOrder(OrderAction action)
        {
            this.cancellationTokenSource = new CancellationTokenSource();

            Contract contract;

            try
            {
                contract = (await this.client.FindContracts(new SearchRequest {Symbol = this.Ticker, NumberOfResults = 1},
                    this.cancellationTokenSource.Token)).First();
            }
            catch (IbException)
            {
                this.HasErrors = true;
                this.ErrorsChanged(this, new DataErrorsChangedEventArgs("Ticker"));
                return;
            }

            try
            {
                await this.client.Accounts.First(account => account.AccountId == this.SelectedAccount)
                    .PlaceMarketOrder(contract, this.Quantity, action, this.cancellationTokenSource.Token);
            }
            catch (IbException)
            {
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == "Ticker")
            {
                yield return "Wrong ticker";
            }
        }

        public bool HasErrors { get; private set; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate {};
    }
}
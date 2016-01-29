using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private IObservableCollection<OrderType> orderTypes;
        private OrderType selectedOrderType;
        private decimal? limitPrice;
        private decimal? stopPrice;

        public PlaceOrderViewModel(IClient client)
        {
            this.DisplayName = "Place market order";
            this.client = client;

            this.Accounts = new BindableCollection<string>();
            this.Accounts.AddRange(client.Accounts.Select(account => account.AccountId));
            this.SelectedAccount = client.Accounts.Select(account => account.AccountId).First();

            this.OrderTypes = new BindableCollection<OrderType> {OrderType.Market, OrderType.Limit, OrderType.Stop};
            this.SelectedOrderType = OrderType.Market;

            this.ControlsEnabled = true;
            this.Quantity = 100;

            this.PropertyChanged += this.OnPropertyChanged;
        }

        public IObservableCollection<OrderType> OrderTypes
        {
            get { return this.orderTypes; }
            set
            {
                if (Equals(value, this.orderTypes)) return;
                this.orderTypes = value;
                this.NotifyOfPropertyChange(() => this.OrderTypes);
            }
        }

        public OrderType SelectedOrderType
        {
            get { return this.selectedOrderType; }
            set
            {
                if (value == this.selectedOrderType) return;
                this.selectedOrderType = value;
                this.NotifyOfPropertyChange(() => this.SelectedOrderType);
                this.NotifyOfPropertyChange(() => this.LimitPriceEnabled);
                this.NotifyOfPropertyChange(() => this.StopPriceEnabled);
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

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Specify quantity greater than 0")]
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

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Specify valid ticker")]
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

        public bool LimitPriceEnabled
        {
            get { return this.ControlsEnabled && this.SelectedOrderType == OrderType.Limit; }
        }

        public bool StopPriceEnabled
        {
            get { return this.ControlsEnabled && this.SelectedOrderType == OrderType.Stop; }
        }

        public decimal? LimitPrice
        {
            get { return this.limitPrice; }
            set
            {
                if (value == this.limitPrice) return;
                this.limitPrice = value;
                this.NotifyOfPropertyChange(() => this.LimitPrice);
            }
        }

        public decimal? StopPrice
        {
            get { return this.stopPrice; }
            set
            {
                if (value == this.stopPrice) return;
                this.stopPrice = value;
                this.NotifyOfPropertyChange(() => this.StopPrice);
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
            if (this.SelectedOrderType == OrderType.Limit && !this.LimitPrice.HasValue)
            {
                this.SetFieldError(true, "LimitPrice");
                return;
            }

            if (this.SelectedOrderType == OrderType.Stop && !this.StopPrice.HasValue)
            {
                this.SetFieldError(true, "StopPrice");
                return;
            }

            this.cancellationTokenSource = new CancellationTokenSource();

            var searchRequest = new SearchRequest{NumberOfResults = 1};

            if (this.Ticker.Length == 7 && this.Ticker[3] == '.')
            {
                searchRequest.LocalSymbol = this.Ticker;
                searchRequest.SecurityType = SecurityType.CASH;
            }
            else
            {
                searchRequest.Symbol = this.Ticker;
            }

            Contract contract;

            try
            {
                contract = (await this.client.FindContracts(searchRequest,
                    this.cancellationTokenSource.Token)).First();
            }
            catch (IbException)
            {
                this.SetFieldError(true, "Ticker");
                return;
            }

            try
            {
                var orderParams = new OrderParams
                {
                    Contract = contract,
                    LimitPrice = this.LimitPrice,
                    OrderAction = action,
                    OrderType = this.SelectedOrderType,
                    Quantity = this.Quantity,
                    StopPrice = this.StopPrice,
                };

                var account = this.client.Accounts.First(acc => acc.AccountId == this.SelectedAccount);

                await account.PlaceOrder(orderParams, this.cancellationTokenSource.Token);

            }
            catch (IbException)
            {
            }
        }

        private void SetFieldError(bool hasError, string propertyName)
        {
            this.HasErrors = hasError;
            this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == "Ticker")
            {
                yield return "Wrong ticker";
            }

            if (propertyName == "LimitPrice")
            {
                yield return "Wrong limit price";
            }

            if (propertyName == "StopPrice")
            {
                yield return "Wrong stop price";
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Ticker")
            {
                this.SetFieldError(false, "Ticker");
            }

            if (propertyChangedEventArgs.PropertyName == "LimitPrice" || propertyChangedEventArgs.PropertyName == "SelectedOrderType")
            {
                this.SetFieldError(false, "LimitPrice");
            }

            if (propertyChangedEventArgs.PropertyName == "StopPrice" || propertyChangedEventArgs.PropertyName == "SelectedOrderType")
            {
                this.SetFieldError(false, "StopPrice");
            }
        }

        public bool HasErrors { get; private set; }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate {};
    }
}
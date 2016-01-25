using System;
using Caliburn.Micro;
using IBApi.Executions;

namespace Sample.Executions
{
    internal class ExecutionView : PropertyChangedBase
    {
        private string account;
        private double averagePrice;
        private bool buy;
        private int cumQuantity;
        private string exchange;
        private string id;
        private int orderId;
        private double price;
        private int quantity;
        private string symbol;
        private DateTime time;

        public ExecutionView(Execution execution)
        {
            this.Symbol = execution.Contract.Symbol;
            this.Id = execution.Id;
            this.Account = execution.Account;
            this.Buy = execution.Buy;
            this.Exchange = execution.Exchange;
            this.OrderId = execution.OrderId;
            this.AveragePrice = execution.AveragePrice;
            this.Price = execution.Price;
            this.CumQuantity = execution.CumQuantity;
            this.Quantity = execution.Quantity;
            this.Time = execution.Time;
        }

        public string Symbol
        {
            get { return this.symbol; }
            set
            {
                if (value == this.symbol) return;
                this.symbol = value;
                this.NotifyOfPropertyChange(() => this.Symbol);
            }
        }

        public string Id
        {
            get { return this.id; }
            set
            {
                if (value == this.id) return;
                this.id = value;
                this.NotifyOfPropertyChange(() => this.Id);
            }
        }

        public string Account
        {
            get { return this.account; }
            set
            {
                if (value == this.account) return;
                this.account = value;
                this.NotifyOfPropertyChange(() => this.Account);
            }
        }

        public bool Buy
        {
            get { return this.buy; }
            set
            {
                if (value == this.buy) return;
                this.buy = value;
                this.NotifyOfPropertyChange(() => this.Buy);
            }
        }

        public string Exchange
        {
            get { return this.exchange; }
            set
            {
                if (value == this.exchange) return;
                this.exchange = value;
                this.NotifyOfPropertyChange(() => this.Exchange);
            }
        }

        public int OrderId
        {
            get { return this.orderId; }
            set
            {
                if (value == this.orderId) return;
                this.orderId = value;
                this.NotifyOfPropertyChange(() => this.OrderId);
            }
        }

        public double AveragePrice
        {
            get { return this.averagePrice; }
            set
            {
                if (value.Equals(this.averagePrice)) return;
                this.averagePrice = value;
                this.NotifyOfPropertyChange(() => this.AveragePrice);
            }
        }

        public double Price
        {
            get { return this.price; }
            set
            {
                if (value.Equals(this.price)) return;
                this.price = value;
                this.NotifyOfPropertyChange(() => this.Price);
            }
        }

        public int CumQuantity
        {
            get { return this.cumQuantity; }
            set
            {
                if (value == this.cumQuantity) return;
                this.cumQuantity = value;
                this.NotifyOfPropertyChange(() => this.CumQuantity);
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

        public DateTime Time
        {
            get { return this.time; }
            set
            {
                if (value.Equals(this.time)) return;
                this.time = value;
                this.NotifyOfPropertyChange(() => this.Time);
            }
        }
    }
}
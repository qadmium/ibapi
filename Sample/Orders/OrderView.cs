using System.Windows.Controls.Primitives;
using Caliburn.Micro;
using IBApi.Orders;

namespace Sample.Orders
{
    internal class OrderView : PropertyChangedBase
    {
        private string account;
        private OrderAction action;
        private int? filledQuantity;
        private int quantity;
        private string symbol;
        private double? limitPrice;
        private double? stopPrice;
        private OrderState state;
        private string exchange;

        public OrderView(IOrder order)
        {
            order.OrderChanged += this.OrderChanged;
            this.Account = order.Account;
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

        public OrderAction Action
        {
            get { return this.action; }
            set
            {
                if (value == this.action) return;
                this.action = value;
                this.NotifyOfPropertyChange(() => this.Action);
            }
        }

        public int? FilledQuantity
        {
            get { return this.filledQuantity; }
            set
            {
                if (value == this.filledQuantity) return;
                this.filledQuantity = value;
                this.NotifyOfPropertyChange(() => this.FilledQuantity);
            }
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

        private void OrderChanged(object sender, OrderChangedEventArgs orderChangedEventArgs)
        {
            var order = orderChangedEventArgs.Order;

            this.Action = order.Action;
            this.Quantity = order.Quantity;
            this.Symbol = order.Contract.Symbol;
            this.FilledQuantity = order.FilledQuantity;
            this.LimitPrice = order.LimitPrice;
            this.StopPrice = order.StopPrice;
            this.State = order.State;
            this.Exchange = order.Route;
        }

        public OrderState State
        {
            get { return this.state; }
            set
            {
                if (value == this.state) return;
                this.state = value;
                this.NotifyOfPropertyChange(() => this.State);
            }
        }

        public double? StopPrice
        {
            get { return this.stopPrice; }
            set
            {
                if (value.Equals(this.stopPrice)) return;
                this.stopPrice = value;
                this.NotifyOfPropertyChange(() => this.StopPrice);
            }
        }

        public double? LimitPrice
        {
            get { return this.limitPrice; }
            set
            {
                if (value.Equals(this.limitPrice)) return;
                this.limitPrice = value;
                this.NotifyOfPropertyChange(() => this.LimitPrice);
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
    }
}
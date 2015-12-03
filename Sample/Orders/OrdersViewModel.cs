using System.Linq;
using Caliburn.Micro;
using IBApi;
using IBApi.Orders;

namespace Sample.Orders
{
    internal class OrdersViewModel : Screen
    {
        private IObservableCollection<OrderView> orders;

        public OrdersViewModel(IClient client)
        {
            this.DisplayName = "Orders";

            this.orders = new BindableCollection<OrderView>();

            foreach (var order in client.Accounts.SelectMany(account => account.OrdersStorage.Orders))
            {
                this.Orders.Add(new OrderView(order));
            }

            foreach (var account in client.Accounts)
            {
                account.OrdersStorage.OrderAdded += this.OnOrderAdded;
            }
        }

        public IObservableCollection<OrderView> Orders
        {
            get { return this.orders; }
            set
            {
                if (Equals(value, this.orders)) return;
                this.orders = value;
                this.NotifyOfPropertyChange(() => this.Orders);
            }
        }

        private void OnOrderAdded(object sender, OrderAddedEventArgs orderAddedEventArgs)
        {
            this.Orders.Add(new OrderView(orderAddedEventArgs.Order));
        }
    }
}
using Caliburn.Micro;
using IBApi;
using Sample.Accounts;
using Sample.Orders;
using Sample.PlaceOrder;
using Sample.Positions;

namespace Sample.Shell
{
    internal class ShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly IWindowManager windowManager;
        private readonly IClient client;
        private readonly PlaceOrderViewModel placeOrderView;

        public ShellViewModel(IWindowManager windowManager, AccountsViewModel accounts, PositionsViewModel positions,
            OrdersViewModel orders, IClient client, PlaceOrderViewModel placeOrder)
        {
            this.windowManager = windowManager;
            this.client = client;
            this.placeOrderView = placeOrder;
            this.Items.Add(accounts);
            this.Items.Add(positions);
            this.Items.Add(orders);
        }

        public void PlaceOrder()
        {
            this.windowManager.ShowDialog(this.placeOrderView);
        }
    }
}
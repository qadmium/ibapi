using Caliburn.Micro;
using IBApi;
using Sample.Accounts;
using Sample.Executions;
using Sample.MessageBox;
using Sample.Orders;
using Sample.PlaceOrder;
using Sample.Positions;

namespace Sample.Shell
{
    internal class ShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly IClient client;
        private readonly MessageBoxViewModelFactory messageBoxFactory;
        private readonly PlaceOrderViewModel placeOrderView;
        private readonly IWindowManager windowManager;

        public ShellViewModel(IWindowManager windowManager, AccountsViewModel accounts, PositionsViewModel positions,
            OrdersViewModel orders, ExecutionsViewModel executions, IClient client, PlaceOrderViewModel placeOrder,
            MessageBoxViewModelFactory messageBoxFactory)
        {
            this.windowManager = windowManager;
            this.client = client;
            this.placeOrderView = placeOrder;
            this.messageBoxFactory = messageBoxFactory;
            this.Items.Add(accounts);
            this.Items.Add(positions);
            this.Items.Add(orders);
            this.Items.Add(executions);

            this.client.ConnectionDisconnected += this.ClientOnConnectionDisconnected;
        }

        private void ClientOnConnectionDisconnected(object sender, DisconnectedEventArgs disconnectedEventArgs)
        {
            this.windowManager.ShowDialog(this.messageBoxFactory.Create(disconnectedEventArgs.Reason));
            this.TryClose();
        }

        public void PlaceOrder()
        {
            this.windowManager.ShowDialog(this.placeOrderView);
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                this.client.Dispose();
            }

            base.OnDeactivate(close);
        }
    }
}
using IBApi.Accounts;
using IBApi.Executions;
using IBApi.Orders;
using IBApi.Positions;

namespace IBApi.Infrastructure
{
    internal sealed class ProxiesFactory
    {
        private readonly Dispatcher dispatcher;

        public ProxiesFactory(Dispatcher dispatcher)
        {
            System.Diagnostics.Contracts.Contract.Requires(dispatcher != null);
            this.dispatcher = dispatcher;
        }

        public IAccount CreateAccountProxy(IAccountInternal account)
        {
            System.Diagnostics.Contracts.Contract.Requires(account != null);
            return new AccountProxy(this.dispatcher, account, this.CreateOrdersStorageProxy(account.OrdersStorage),
                this.CreateExecutionsStorageProxy(account.ExecutionsStorage),
                this.CreatePositionsStorage(account.PositionsStorage));
        }

        private IOrdersStorage CreateOrdersStorageProxy(IOrdersStorage ordersStorage)
        {
            return new OrdersStorageProxy(ordersStorage, this.dispatcher, this);
        }

        private IExecutionsStorage CreateExecutionsStorageProxy(IExecutionsStorage executionsStorage)
        {
            return new ExecutionsStorageProxy(executionsStorage, this.dispatcher);
        }

        private IPositionsStorage CreatePositionsStorage(IPositionsStorage positionsStorage)
        {
            return new PositionsStorageProxy(positionsStorage, this.dispatcher, this);
        }

        public IPosition CreatePositionProxy(IPosition position)
        {
            System.Diagnostics.Contracts.Contract.Requires(position != null);
            return new PositionsProxy(position, this.dispatcher);
        }

        public IOrder CreateOrderProxy(IOrder order)
        {
            System.Diagnostics.Contracts.Contract.Requires(order != null);
            return new OrdersProxy(order, this.dispatcher);
        }
    }
}
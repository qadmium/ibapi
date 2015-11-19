using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using IBApi.Connection;
using IBApi.Messages.Server;

namespace IBApi.Orders
{
    internal class OrdersStorage : IOrdersStorageInternal
    {
        private readonly string accountName;

        private readonly IApiObjectsFactory objectsFactory;
        private readonly Dictionary<int, Order> orders = new Dictionary<int, Order>();
        private IDisposable subscription;

        public OrdersStorage(IConnection connection, IApiObjectsFactory objectsFactory, string accountName)
        {
            Contract.Requires(connection != null);
            Contract.Requires(objectsFactory != null);

            this.objectsFactory = objectsFactory;
            this.accountName = accountName;

            this.Subscribe(connection);
        }

        public event OrderAddedEventHandler OrderAdded = delegate { };

        public IReadOnlyCollection<IOrder> Orders
        {
            get
            {
                var castedOrders = this.orders.Values.Select<Order, IOrder>(order => order).ToList();
                return castedOrders.AsReadOnly();
            }
        }

        public void Dispose()
        {
            this.subscription.Dispose();
        }

        private void Subscribe(IConnection connection)
        {
            Contract.Requires(connection != null);
            this.subscription = connection.Subscribe((OpenOrderMessage message) => message.Account == this.accountName,
                this.OnNewOrder);
        }

        private void OnNewOrder(OpenOrderMessage message)
        {
            Order order;

            if (this.orders.TryGetValue(message.OrderId, out order))
            {
                order.Update(message);
                return;
            }

            order = this.objectsFactory.CreateOrder(message.OrderId, this.accountName);
            order.Update(message);
            this.orders[message.OrderId] = order;
            this.OrderAdded(order);
        }
    }
}
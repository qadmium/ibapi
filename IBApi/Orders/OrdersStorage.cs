using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Messages.Server;

namespace IBApi.Orders
{
    class OrdersStorage : IOrdersStorageInternal
    {
        public event InitializedEventHandler Initialized;
        public bool IsInitialized { get; private set; }

        public event OrderAddedEventHandler OrderAdded = delegate { };

        public ReadOnlyCollection<IOrder> Orders
        {
            get
            {
                var castedOrders = orders.Values.Select<Order, IOrder>(order => order).ToList();
                return new ReadOnlyCollection<IOrder>(castedOrders);
            }
        }

        public OrdersStorage(IConnection connection, IApiObjectsFactory objectsFactory, string accountName)
        {
            this.objectsFactory = objectsFactory;
            this.accountName = accountName;

            Subscribe(connection);
        }

        public void AccountsReceived()
        {
            IsInitialized = true;
            Initialized();
        }

        public void Dispose()
        {
            subscription.Dispose();
        }

        private void Subscribe(IConnection connection)
        {
            subscription = connection.Subscribe((OpenOrderMessage message) => message.Account == accountName, OnNewOrder);
        }

        private void OnNewOrder(OpenOrderMessage message)
        {
            Order order;

            if (orders.TryGetValue(message.OrderId, out order))
            {
                order.Update(message);
                return;
            }

            order = objectsFactory.CreateOrder(message.OrderId);
            order.Update(message);
            orders[message.OrderId] = order;
            OrderAdded(order);
        }

        private readonly IApiObjectsFactory objectsFactory;
        private readonly string accountName;
        private readonly Dictionary<int, Order> orders = new Dictionary<int, Order>();
        private IDisposable subscription;
    }
}
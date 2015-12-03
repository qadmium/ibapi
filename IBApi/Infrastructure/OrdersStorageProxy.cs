using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using IBApi.Orders;

namespace IBApi.Infrastructure
{
    internal sealed class OrdersStorageProxy : IOrdersStorage
    {
        private readonly Dispatcher dispatcher;
        private readonly ProxiesFactory proxiesFactory;
        private readonly object orderAddedEvent;
        private readonly List<IOrder> orders;

        public OrdersStorageProxy(IOrdersStorage ordersStorage, Dispatcher dispatcher, ProxiesFactory proxiesFactory)
        {
            Contract.Requires(ordersStorage != null);
            Contract.Requires(dispatcher != null);
            Contract.Requires(proxiesFactory != null);

            this.dispatcher = dispatcher;
            this.proxiesFactory = proxiesFactory;

            this.orderAddedEvent = this.dispatcher.RegisterEvent();
            ordersStorage.OrderAdded += this.OnOrderAdded;
            this.orders = ordersStorage.Orders.Select(proxiesFactory.CreateOrderProxy).ToList();
        }

        private void OnOrderAdded(object sender, OrderAddedEventArgs orderAddedEventArgs)
        {
            var proxy = this.proxiesFactory.CreateOrderProxy(orderAddedEventArgs.Order);
            this.orders.Add(proxy);

            this.dispatcher.RaiseEvent(this.orderAddedEvent, this, new OrderAddedEventArgs{Order = proxy});
        }

        public event EventHandler<OrderAddedEventArgs> OrderAdded
        {
            add { this.dispatcher.AddHandler(this.orderAddedEvent, value); }
            remove { this.dispatcher.RemoveHandler(this.orderAddedEvent, value); }
        }

        public IReadOnlyCollection<IOrder> Orders
        {
            get { return this.dispatcher.Dispatch(() => this.orders.AsReadOnly()); }
        }
    }
}
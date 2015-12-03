using System;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Contracts;
using IBApi.Orders;

namespace IBApi.Infrastructure
{
    internal class OrdersProxy : IOrder
    {
        private readonly Dispatcher dispatcher;
        private readonly IOrder order;
        private readonly object orderChangedEvent;

        public OrdersProxy(IOrder order, Dispatcher dispatcher)
        {
            System.Diagnostics.Contracts.Contract.Requires(order != null);
            System.Diagnostics.Contracts.Contract.Requires(dispatcher != null);

            this.order = order;
            this.dispatcher = dispatcher;

            this.order.OrderChanged += this.OnOrderChanged;
            this.orderChangedEvent = this.dispatcher.RegisterEvent();
        }

        public event EventHandler<OrderChangedEventArgs> OrderChanged
        {
            add { this.dispatcher.AddHandler(this.orderChangedEvent, value); }
            remove { this.dispatcher.RemoveHandler(this.orderChangedEvent, value); }
        }

        public string Account
        {
            get { return this.dispatcher.Dispatch(() => this.order.Account); }
        }

        public int Id
        {
            get { return this.dispatcher.Dispatch(() => this.order.Id); }
        }

        public Contract Contract
        {
            get { return this.dispatcher.Dispatch(() => this.order.Contract); }
        }

        public OrderState State
        {
            get { return this.dispatcher.Dispatch(() => this.order.State); }
        }

        public OrderAction Action
        {
            get { return this.dispatcher.Dispatch(() => this.order.Action); }
        }

        public OrderType Type
        {
            get { return this.dispatcher.Dispatch(() => this.order.Type); }
        }

        public double? LimitPrice
        {
            get { return this.dispatcher.Dispatch(() => this.order.LimitPrice); }
        }

        public double? StopPrice
        {
            get { return this.dispatcher.Dispatch(() => this.order.StopPrice); }
        }

        public int Quantity
        {
            get { return this.dispatcher.Dispatch(() => this.order.Quantity); }
        }

        public int? FilledQuantity
        {
            get { return this.dispatcher.Dispatch(() => this.order.FilledQuantity); }
        }

        public int? RemainingQuantity
        {
            get { return this.dispatcher.Dispatch(() => this.order.RemainingQuantity); }
        }

        public double? AverageFillPrice
        {
            get { return this.dispatcher.Dispatch(() => this.order.AverageFillPrice); }
        }

        public int PermId
        {
            get { return this.dispatcher.Dispatch(() => this.order.PermId); }
        }

        public int? ParentId
        {
            get { return this.dispatcher.Dispatch(() => this.order.ParentId); }
        }

        public double? LastFillPrice
        {
            get { return this.dispatcher.Dispatch(() => this.order.LastFillPrice); }
        }

        public int ClientId
        {
            get { return this.dispatcher.Dispatch(() => this.order.ClientId); }
        }

        public string Route
        {
            get { return this.dispatcher.Dispatch(() => this.order.Route); }
        }

        public int? DisplaySize
        {
            get { return this.dispatcher.Dispatch(() => this.order.DisplaySize); }
        }

        public Task WaitForFill(CancellationToken cancellationToken)
        {
            return this.dispatcher.Dispatch(() => this.order.WaitForFill(cancellationToken));
        }

        private void OnOrderChanged(object sender, OrderChangedEventArgs orderChangedEventArgs)
        {
            this.dispatcher.RaiseEvent(this.orderChangedEvent, this, new OrderChangedEventArgs {Order = this});
        }
    }
}
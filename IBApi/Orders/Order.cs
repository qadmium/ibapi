using System;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Messages.Server;
using IBApi.Orders.OrderStateExtensions;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Orders
{
    internal sealed class Order : IOrder, IDisposable
    {
        public event OrderChangedEventHandler OrderChanged = delegate { };

        public int Id { get; private set; }
        public Contract Contract { get; private set; }
        public OrderState State { get; private set; }
        public OrderAction Action { get; private set; }
        public OrderType Type { get; private set; }
        public double? LimitPrice { get; private set; }
        public double? StopPrice { get; private set; }
        public int Quantity { get; private set; }
        public int? FilledQuantity { get; private set; }
        public int? RemainingQuantity { get; private set; }
        public double? AverageFillPrice { get; private set; }
        public int PermId { get; private set; }
        public int? ParentId { get; private set; }
        public double? LastFillPrice { get; private set; }
        public int ClientId { get; private set; }
        public string Route { get; private set; }
        public int? DisplaySize { get; private set; }

        public Order(int id, IConnection connection)
        {
            Id = id;
            Subscribe(connection);
        }

        private void Subscribe(IConnection connection)
        {
            subscription = connection.Subscribe((OrderStatusMessage message) => message.OrderId == Id, OnStatusUpdate);
        }

        public void Update(OpenOrderMessage message)
        {
            CodeContract.Requires<InvalidOperationException>(message.OrderId == Id);

            State = message.Status.ToOrderState();
            Action = message.OrderAction.ToOrderAction();
            Type = message.OrderType.ToOrderType();
            LimitPrice = message.LimitPrice;
            StopPrice = message.AuxPrice;
            Quantity = message.TotalQuantity;
            PermId = message.PermId;
            ParentId = message.ParentId;
            ClientId = message.ClientId;
            Contract = Contract.FromOpenOrderMessage(message);
            Route = message.Exchange;
            DisplaySize = message.DisplaySize;

            OrderChanged(this);
        }

        public void Dispose()
        {
            subscription.Dispose();
        }

        private void OnStatusUpdate(OrderStatusMessage message)
        {
            CodeContract.Requires<InvalidOperationException>(message.OrderId == Id);

            State = message.Status.ToOrderState();
            FilledQuantity = message.Filled;
            RemainingQuantity = message.Remaining;
            AverageFillPrice = message.AverageFillPrice;
            PermId = message.PermId;
            ParentId = message.ParentId;
            LastFillPrice = message.LastFillPrice;
            ClientId = message.ClientId;

            OrderChanged(this);
        }

        private IDisposable subscription;
    }
}
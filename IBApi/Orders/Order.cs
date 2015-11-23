using System;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Messages.Server;
using IBApi.Orders.OrderExtensions;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Orders
{
    internal sealed class Order : IOrder, IDisposable
    {
        private readonly IApiObjectsFactory factory;
        private IDisposable subscription;

        public Order(int id, string account, IConnection connection, IApiObjectsFactory factory)
        {
            CodeContract.Requires(connection != null);
            CodeContract.Requires(factory != null);

            this.factory = factory;
            this.Id = id;
            this.Subscribe(connection);
            this.Account = account;
        }

        public void Dispose()
        {
            this.subscription.Dispose();
        }

        public event OrderChangedEventHandler OrderChanged = delegate { };

        public string Account { get; private set; }
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
        public Task WaitForFill(CancellationToken cancellationToken)
        {
            return this.factory.CreateWaitForOrderFillOperation(this, cancellationToken);
        }

        private void Subscribe(IConnection connection)
        {
            CodeContract.Requires(connection != null);
            this.subscription = connection.Subscribe((OrderStatusMessage message) => message.OrderId == this.Id,
                this.OnStatusUpdate);
        }

        public void Update(OpenOrderMessage message)
        {
            CodeContract.Requires(message.OrderId == this.Id);

            this.State = message.Status.ToOrderState();
            this.Action = message.OrderAction.ToOrderAction();
            this.Type = message.OrderType.ToOrderType();
            this.LimitPrice = message.LimitPrice;
            this.StopPrice = message.AuxPrice;
            this.Quantity = message.TotalQuantity;
            this.PermId = message.PermId;
            this.ParentId = message.ParentId;
            this.ClientId = message.ClientId;
            this.Contract = Contract.FromOpenOrderMessage(message);
            this.Route = message.Exchange;
            this.DisplaySize = message.DisplaySize;

            this.OrderChanged(this);
        }

        private void OnStatusUpdate(OrderStatusMessage message)
        {
            CodeContract.Requires(message.OrderId == this.Id);

            this.State = message.Status.ToOrderState();
            this.FilledQuantity = message.Filled;
            this.RemainingQuantity = message.Remaining;
            this.AverageFillPrice = message.AverageFillPrice;
            this.PermId = message.PermId;
            this.ParentId = message.ParentId;
            this.LastFillPrice = message.LastFillPrice;
            this.ClientId = message.ClientId;

            this.OrderChanged(this);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Errors;
using IBApi.Messages.Server;
using IBApi.Orders.OrderExtensions;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Orders
{
    internal sealed class Order : IOrder, IDisposable
    {
        private readonly IApiObjectsFactory factory;
        private readonly CancellationTokenSource internaCancellationTokenSource;
        private readonly List<IDisposable> subscriptions = new List<IDisposable>();

        public Order(int id, string account, IConnection connection, IApiObjectsFactory factory, CancellationTokenSource internaCancellationTokenSource)
        {
            CodeContract.Requires(connection != null);
            CodeContract.Requires(factory != null);

            this.factory = factory;
            this.internaCancellationTokenSource = internaCancellationTokenSource;
            this.Id = id;
            this.Subscribe(connection);
            this.Account = account;
        }

        public void Dispose()
        {
            this.subscriptions.Unsubscribe();
        }

        public event EventHandler<OrderChangedEventArgs> OrderChanged = delegate { };

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
        public string LastError { get; private set; }
        public ErrorCode? LastErrorCode { get; private set; }
        public Task WaitForFill(CancellationToken cancellationToken)
        {
            using (var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.internaCancellationTokenSource.Token, cancellationToken))
            {
                return this.factory.CreateWaitForOrderFillOperation(this, cancellationTokenSource.Token);
            }
        }

        private void Subscribe(IConnection connection)
        {
            CodeContract.Requires(connection != null);
            this.subscriptions.AddRange(new []
            {
                connection.Subscribe((OrderStatusMessage message) => message.OrderId == this.Id,
                this.OnStatusUpdate),

                connection.Subscribe((ErrorMessage message) => message.RequestId == this.Id,
                this.OnError)
            });
        }

        private void OnError(ErrorMessage errorMessage)
        {
            this.LastError = errorMessage.Message;
            this.LastErrorCode = errorMessage.ErrorCode;

            if (this.LastErrorCode == ErrorCode.OrderRejected)
            {
                this.State = OrderState.Rejected;
            }

            this.OrderChanged(this, new OrderChangedEventArgs{Order = this});
        }

        public void Update(OpenOrderMessage message)
        {
            CodeContract.Requires(message.OrderId == this.Id);
            
            this.UpdateStatus(message.Status);
            
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

            this.OrderChanged(this, new OrderChangedEventArgs{Order = this});
        }

        private void UpdateStatus(string status)
        {
            if (this.State != OrderState.Rejected)
            {
                this.State = status.ToOrderState();
            }
        }

        private void OnStatusUpdate(OrderStatusMessage message)
        {
            CodeContract.Requires(message.OrderId == this.Id);

            this.UpdateStatus(message.Status);

            this.FilledQuantity = message.Filled;
            this.RemainingQuantity = message.Remaining;
            this.AverageFillPrice = message.AverageFillPrice;
            this.PermId = message.PermId;
            this.ParentId = message.ParentId;
            this.LastFillPrice = message.LastFillPrice;
            this.ClientId = message.ClientId;

            this.OrderChanged(this, new OrderChangedEventArgs { Order = this });
        }
    }
}
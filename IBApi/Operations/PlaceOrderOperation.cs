using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Errors;
using IBApi.Exceptions;
using IBApi.Messages.Client;
using IBApi.Orders;

namespace IBApi.Operations
{
    internal sealed class PlaceOrderOperation
    {
        private readonly IOrdersStorageInternal ordersStorage;
        private readonly TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();
        private readonly int orderId;
        private readonly IDisposable subscription;

        public PlaceOrderOperation(RequestPlaceOrderMessage requestPlaceOrderMessage, IConnection connection, IOrdersStorageInternal ordersStorage, CancellationToken cancellationToken)
        {
            Contract.Requires(connection != null);
            Contract.Requires(ordersStorage != null);
            this.orderId = requestPlaceOrderMessage.OrderId;
            this.ordersStorage = ordersStorage;
            this.ordersStorage.OrderAdded += this.OnOrderAdded;

            cancellationToken.Register(() =>
            {
                this.ordersStorage.OrderAdded -= this.OnOrderAdded;
                this.subscription.Dispose();
                this.taskCompletionSource.SetCanceled();
            });

            this.subscription = connection.SubscribeForErrors(error => error.RequestId == this.orderId, this.OnError);

            connection.SendMessage(requestPlaceOrderMessage);
        }

        private void OnError(Error error)
        {
            this.taskCompletionSource.SetException(new IbException(error.Message, error.Code));
        }

        private void OnOrderAdded(IOrder order)
        {
            if (order.Id != this.orderId)
            {
                return;
            }

            this.ordersStorage.OrderAdded -= this.OnOrderAdded;
            this.taskCompletionSource.SetResult(this.orderId);
        }

        public Task<int> Result { get { return this.taskCompletionSource.Task; }}
    }
}

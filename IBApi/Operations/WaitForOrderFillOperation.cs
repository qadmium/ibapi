using System.Threading;
using System.Threading.Tasks;
using IBApi.Orders;

namespace IBApi.Operations
{
    internal sealed class WaitForOrderFillOperation
    {
        private readonly IOrder order;
        private readonly TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

        public WaitForOrderFillOperation(IOrder order, CancellationToken cancellationToken)
        {
            this.order = order;
            order.OrderChanged += this.OnOrderChanged;

            cancellationToken.Register(() =>
            {
                order.OrderChanged -= this.OnOrderChanged;
                this.taskCompletionSource.SetCanceled();
            });
        }

        public Task Result
        {
            get { return this.taskCompletionSource.Task; }
        }

        private void OnOrderChanged(object sender, OrderChangedEventArgs eventArgs)
        {
            if (this.order.FilledQuantity == this.order.Quantity)
            {
                this.order.OrderChanged -= this.OnOrderChanged;
                this.taskCompletionSource.SetResult(true);
            }
        }
    }
}

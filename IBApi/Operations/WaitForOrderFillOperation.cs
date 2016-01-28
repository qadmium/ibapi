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

            if (this.OrderFilled())
            {
                this.taskCompletionSource.SetResult(true);
                return;
            }

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
            if (this.OrderFilled())
            {
                this.order.OrderChanged -= this.OnOrderChanged;
                this.taskCompletionSource.SetResult(true);
            }
        }

        private bool OrderFilled()
        {
            return this.order.FilledQuantity == this.order.Quantity;
        }
    }
}

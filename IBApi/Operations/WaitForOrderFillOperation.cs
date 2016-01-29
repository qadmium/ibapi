using System.Threading;
using System.Threading.Tasks;
using IBApi.Exceptions;
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

            if (this.OrderInFinalState())
            {
                this.SetResult();
                return;
            }

            order.OrderChanged += this.OnOrderChanged;

            cancellationToken.Register(() =>
            {
                order.OrderChanged -= this.OnOrderChanged;
                this.taskCompletionSource.SetCanceled();
            });
        }

        private void SetResult()
        {
            if (this.order.State == OrderState.Cancelled
                || this.order.State == OrderState.Rejected)
            {
                this.taskCompletionSource.SetException(new IbException(this.order.LastError, this.order.LastErrorCode));
                return;
            }

            this.taskCompletionSource.SetResult(true);
        }

        public Task Result
        {
            get { return this.taskCompletionSource.Task; }
        }

        private void OnOrderChanged(object sender, OrderChangedEventArgs eventArgs)
        {
            if (!this.OrderInFinalState())
            {
                return;
            }

            this.order.OrderChanged -= this.OnOrderChanged;
            this.SetResult();
        }

        private bool OrderInFinalState()
        {
            return this.order.State == OrderState.Filled
                   || this.order.State == OrderState.Cancelled
                   || this.order.State == OrderState.Rejected;
        }
    }
}

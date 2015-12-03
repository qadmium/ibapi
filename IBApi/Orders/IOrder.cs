using System;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Contracts;

namespace IBApi.Orders
{
    public class OrderChangedEventArgs : EventArgs
    {
        public IOrder Order { get; set; }
    }

    public interface IOrder
    {
        event EventHandler<OrderChangedEventArgs> OrderChanged;

        string Account { get; }
        int Id { get; }
        Contract Contract { get; }
        OrderState State { get; }
        OrderAction Action { get; }
        OrderType Type { get; }
        double? LimitPrice { get; }
        double? StopPrice { get; }
        int Quantity { get; }
        int? FilledQuantity { get; }
        int? RemainingQuantity { get; }
        double? AverageFillPrice { get; }
        int PermId { get; }
        int? ParentId { get; }
        double? LastFillPrice { get; }
        int ClientId { get; }
        string Route { get; }
        int? DisplaySize { get; }

        Task WaitForFill(CancellationToken cancellationToken);
    }
}
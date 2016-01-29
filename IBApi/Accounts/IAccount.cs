using System;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Executions;
using IBApi.Orders;
using IBApi.Positions;

namespace IBApi.Accounts
{
    public class AccountChangedEventArgs : EventArgs
    {
        public IAccount Account { get; internal set; }
    }

    public interface IAccount
    {
        event EventHandler<AccountChangedEventArgs> AccountChanged;

        string AccountName { get; }
        string AccountId { get; }
        
        AccountFields AccountFields { get; }
        string[] Currencies { get; }
        AccountFields this[string currency] { get; }

        IOrdersStorage OrdersStorage { get; }
        IExecutionsStorage ExecutionsStorage { get; }
        IPositionsStorage PositionsStorage { get; }

        Task<int> PlaceOrder(OrderParams orderParams, CancellationToken cancellationToken);
    }
}
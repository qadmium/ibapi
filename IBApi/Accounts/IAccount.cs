using System.Threading;
using System.Threading.Tasks;
using IBApi.Executions;
using IBApi.Orders;
using IBApi.Positions;

namespace IBApi.Accounts
{
    public delegate void AccountChangedEventHandler(IAccount account);

    public interface IAccount
    {
        event AccountChangedEventHandler AccountChanged;

        string AccountName { get; }
        string AccountId { get; }
        
        AccountFields AccountFields { get; }
        string[] Currencies { get; }
        AccountFields this[string currency] { get; }

        IOrdersStorage OrdersStorage { get; }
        IExecutionsStorage ExecutionsStorage { get; }
        IPositionsStorage PositionStorage { get; }

        Task<int> PlaceMarketOrder(CancellationToken cancellationToken);
    }
}
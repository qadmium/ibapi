using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Executions;
using IBApi.Orders;
using IBApi.Positions;

namespace IBApi.Accounts
{
    [ContractClassFor(typeof(IAccount))]
    // ReSharper disable once InconsistentNaming
    public abstract class IAccountContract : IAccount
    {
        public abstract event EventHandler<AccountChangedEventArgs> AccountChanged;
        public abstract string AccountName { get; }
        public abstract string AccountId { get; }

        public AccountFields AccountFields
        {
            get
            {
                Contract.Ensures(Contract.Result<AccountFields>() != null);
                return null;
            }
        }

        public abstract string[] Currencies { get; }
        public abstract AccountFields this[string currency] { get; }

        public IOrdersStorage OrdersStorage
        {
            get
            {
                Contract.Ensures(Contract.Result<IOrdersStorage>() != null);
                return null;
            }
        }

        public IExecutionsStorage ExecutionsStorage
        {
            get
            {
                Contract.Ensures(Contract.Result<IExecutionsStorage>() != null);
                return null;
            }
        }

        public IPositionsStorage PositionsStorage
        {
            get
            {
                Contract.Ensures(Contract.Result<IPositionsStorage>() != null);
                return null;
            }
        }

        public Task<int> PlaceOrder(OrderParams orderParams, CancellationToken cancellationToken)
        {
            Contract.Requires((orderParams.OrderType != OrderType.Limit && orderParams.OrderType != OrderType.Limit)
                              || (orderParams.OrderType == OrderType.Limit && orderParams.LimitPrice.HasValue)
                              || (orderParams.OrderType == OrderType.Stop && orderParams.StopPrice.HasValue));
            return null;
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Executions;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Orders;
using IBApi.Orders.OrderExtensions;
using IBApi.Positions;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Accounts
{
    internal class Account : IAccountInternal
    {
        private readonly AccountCurrenciesFields accountCurrenciesFields;
        private readonly IApiObjectsFactory factory;
        private readonly IExecutionStorageInternal executionStorage;
        private readonly IOrdersStorageInternal ordersStorage;
        private readonly IPositionsStorageInternal positionsesStorage;
        private readonly IDisposable subscription;
        private readonly IIdsDispenser idsDispenser;
        private readonly CancellationTokenSource internalCancelationTokenSource;

        public Account(string name, IConnection connection, IApiObjectsFactory factory, IExecutionStorageInternal executionStorage,
            IPositionsStorageInternal positionsesStorageInternal, IOrdersStorageInternal ordersStorageInternal,
            IIdsDispenser idsDispenser, CancellationTokenSource internalCancelationTokenSource,
            AccountCurrenciesFields accountCurrenciesFields)
        {
            CodeContract.Requires(!string.IsNullOrEmpty(name));
            CodeContract.Requires(connection != null);
            CodeContract.Requires(factory != null);
            CodeContract.Requires(executionStorage != null);
            CodeContract.Requires(positionsesStorageInternal != null);
            CodeContract.Requires(ordersStorageInternal != null);
            CodeContract.Requires(accountCurrenciesFields != null);
            CodeContract.Requires(idsDispenser != null);

            this.AccountName = name;
            this.AccountId = name;

            this.factory = factory;
            this.executionStorage = executionStorage;
            this.positionsesStorage = positionsesStorageInternal;
            this.ordersStorage = ordersStorageInternal;
            this.accountCurrenciesFields = accountCurrenciesFields;
            this.idsDispenser = idsDispenser;
            this.internalCancelationTokenSource = internalCancelationTokenSource;

            this.subscription =
                connection.Subscribe((AccountValueMessage message) => message.AccountName == this.AccountName,
                    this.OnAccountValueMessage);
        }

        public event EventHandler<AccountChangedEventArgs> AccountChanged = delegate { };

        public string AccountName { get; private set; }

        public string AccountId { get; private set; }

        public AccountFields AccountFields
        {
            get { return this.accountCurrenciesFields.AccountFields; }
        }

        public string[] Currencies
        {
            get { return this.accountCurrenciesFields.Currencies; }
        }

        public AccountFields this[string currency]
        {
            get { return this.accountCurrenciesFields[currency]; }
        }

        public IOrdersStorage OrdersStorage
        {
            get { return this.ordersStorage; }
        }

        public IExecutionsStorage ExecutionsStorage
        {
            get { return this.executionStorage; }
        }

        public IPositionsStorage PositionsStorage
        {
            get { return this.positionsesStorage; }
        }

        public async Task<int> PlaceOrder(OrderParams orderParams, CancellationToken cancellationToken)
        {
            using (var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.internalCancelationTokenSource.Token, cancellationToken))
            {
                var orderId = this.idsDispenser.NextOrderId();

                var request = RequestPlaceOrderMessage.Default;
                request.Account = this.AccountId;
                request.Action = orderParams.OrderAction.ToStringAction();
                request.TotalQuantity = orderParams.Quantity;
                request.ContractId = orderParams.Contract.ContractId;
                request.Symbol = orderParams.Contract.Symbol;
                request.SecType = orderParams.Contract.SecurityType.ToString();
                request.Expity = orderParams.Contract.Expiry;
                request.Strike = orderParams.Contract.Strike;
                request.Right = orderParams.Contract.RightString;
                request.Multiplier = orderParams.Contract.AdditionalContractInfo.Multiplier;
                request.Exchange = orderParams.Contract.AdditionalContractInfo.Exchange;
                request.PrimaryExchange = orderParams.Contract.AdditionalContractInfo.PrimaryExchange;
                request.Currency = orderParams.Contract.AdditionalContractInfo.Currency;
                request.LocalSymbol = orderParams.Contract.LocalSymbol;
                request.TradingClass = orderParams.Contract.AdditionalContractInfo.TradingClass;
                request.OrderType = orderParams.OrderType.ToOrderString();
                request.OrderId = orderId;
                request.Transmit = true;

                // ReSharper disable PossibleInvalidOperationException
                // Relations between order type and prices fields checked in contracts
                if (orderParams.OrderType == OrderType.Limit)
                {
                    
                    request.LimitPrice = (double)orderParams.LimitPrice.Value;
                }

                if (orderParams.OrderType == OrderType.Stop)
                {
                    request.AuxPrice = (double)orderParams.StopPrice.Value;
                }
                // ReSharper restore PossibleInvalidOperationException

                return await this.factory.CreatePlaceOrderOperation(request, this.ordersStorage, cancellationTokenSource.Token);
            }
        }

        public void Dispose()
        {
            this.subscription.Dispose();
            this.executionStorage.Dispose();
        }

        private void OnAccountValueMessage(AccountValueMessage message)
        {
            this.accountCurrenciesFields.Update(AccountValue.FromMessage(message));
            this.AccountChanged(this, new AccountChangedEventArgs{Account = this});
        }
    }
}
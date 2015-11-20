using System;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Contracts;
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
        private readonly IPositionsStorageInternal positionsStorage;
        private readonly IDisposable subscription;
        private readonly IIdsDispenser idsDispenser;

        public Account(string name, IConnection connection, IApiObjectsFactory factory, IExecutionStorageInternal executionStorage,
            IPositionsStorageInternal positionsStorageInternal, IOrdersStorageInternal ordersStorageInternal,
            IIdsDispenser idsDispenser,
            AccountCurrenciesFields accountCurrenciesFields)
        {
            CodeContract.Requires(!string.IsNullOrEmpty(name));
            CodeContract.Requires(connection != null);
            CodeContract.Requires(factory != null);
            CodeContract.Requires(executionStorage != null);
            CodeContract.Requires(positionsStorageInternal != null);
            CodeContract.Requires(ordersStorageInternal != null);
            CodeContract.Requires(accountCurrenciesFields != null);
            CodeContract.Requires(idsDispenser != null);

            this.AccountName = name;
            this.AccountId = name;

            this.factory = factory;
            this.executionStorage = executionStorage;
            this.positionsStorage = positionsStorageInternal;
            this.ordersStorage = ordersStorageInternal;
            this.accountCurrenciesFields = accountCurrenciesFields;
            this.idsDispenser = idsDispenser;

            this.subscription =
                connection.Subscribe((AccountValueMessage message) => message.AccountName == this.AccountName,
                    this.OnAccountValueMessage);
        }

        public event AccountChangedEventHandler AccountChanged = delegate { };

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

        public IPositionsStorage PositionStorage
        {
            get { return this.positionsStorage; }
        }

        public async Task<int> PlaceMarketOrder(Contract contract, int quantity, OrderAction action, CancellationToken cancellationToken)
        {
            var orderId = await this.idsDispenser.NextId(cancellationToken);

            var request = RequestPlaceOrderMessage.Default;
            request.Account = this.AccountId;
            request.Action = action.ToStringAction();
            request.TotalQuantity = quantity;
            request.ContractId = contract.ContractId;
            request.Symbol = contract.Symbol;
            request.SecType = contract.SecurityType.ToString();
            request.Expity = contract.Expiry;
            request.Strike = contract.Strike;
            request.Right = contract.RightString;
            request.Multiplier = contract.AdditionalContractInfo.Multiplier;
            request.Exchange = contract.AdditionalContractInfo.Exchange;
            request.PrimaryExchange = contract.AdditionalContractInfo.PrimaryExchange;
            request.Currency = contract.AdditionalContractInfo.Currency;
            request.LocalSymbol = contract.LocalSymbol;
            request.TradingClass = contract.AdditionalContractInfo.TradingClass;
            request.OrderType = OrderType.Market.ToOrderString();
            request.OrderId = orderId;
            request.Transmit = true;

            return await this.factory.CreatePlaceOrderOperation(request, this.ordersStorage, cancellationToken);
        }

        public void Dispose()
        {
            this.subscription.Dispose();
            this.executionStorage.Dispose();
        }

        private void OnAccountValueMessage(AccountValueMessage message)
        {
            this.accountCurrenciesFields.Update(AccountValue.FromMessage(message));
            this.AccountChanged(this);
        }
    }
}
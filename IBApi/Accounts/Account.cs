using System;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Connection;
using IBApi.Executions;
using IBApi.Messages.Server;
using IBApi.Orders;
using IBApi.Positions;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Accounts
{
    internal class Account : IAccountInternal
    {
        private readonly AccountCurrenciesFields accountCurrenciesFields;
        private readonly IExecutionStorageInternal executionStorage;
        private readonly IOrdersStorageInternal ordersStorage;
        private readonly IPositionsStorageInternal positionsStorage;
        private readonly IDisposable subscription;
        private readonly IOrdersIdsDispenser ordersIdsDispenser;

        public Account(string name, IConnection connection, IExecutionStorageInternal executionStorage,
            IPositionsStorageInternal positionsStorageInternal, IOrdersStorageInternal ordersStorageInternal,
            IOrdersIdsDispenser ordersIdsDispenser,
            AccountCurrenciesFields accountCurrenciesFields)
        {
            CodeContract.Requires(!string.IsNullOrEmpty(name));
            CodeContract.Requires(connection != null);
            CodeContract.Requires(executionStorage != null);
            CodeContract.Requires(positionsStorageInternal != null);
            CodeContract.Requires(ordersStorageInternal != null);
            CodeContract.Requires(accountCurrenciesFields != null);
            CodeContract.Requires(ordersIdsDispenser != null);

            this.AccountName = name;
            this.AccountId = name;

            this.executionStorage = executionStorage;
            this.positionsStorage = positionsStorageInternal;
            this.ordersStorage = ordersStorageInternal;
            this.accountCurrenciesFields = accountCurrenciesFields;
            this.ordersIdsDispenser = ordersIdsDispenser;

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

        public async Task<int> PlaceOrder(CancellationToken cancellationToken)
        {
            var orderId = await this.ordersIdsDispenser.NextOrderId(cancellationToken);
            
            return orderId;
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
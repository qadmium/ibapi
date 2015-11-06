using System;
using System.Collections.Generic;
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
        private ICollection<IDisposable> subscriptions = new List<IDisposable>();

        public Account(string name, IConnection connection, IExecutionStorageInternal executionStorage,
            IPositionsStorageInternal positionsStorageInternal, IOrdersStorageInternal ordersStorageInternal,
            AccountCurrenciesFields accountCurrenciesFields)
        {
            CodeContract.Requires(!string.IsNullOrEmpty(name));
            CodeContract.Requires(connection != null);
            CodeContract.Requires(executionStorage != null);
            CodeContract.Requires(positionsStorageInternal != null);
            CodeContract.Requires(ordersStorageInternal != null);
            CodeContract.Requires(accountCurrenciesFields != null);

            this.AccountName = name;
            this.AccountId = name;

            this.executionStorage = executionStorage;
            this.positionsStorage = positionsStorageInternal;
            this.ordersStorage = ordersStorageInternal;
            this.accountCurrenciesFields = accountCurrenciesFields;

            this.Subscribe(connection);
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

        public void Dispose()
        {
            this.subscriptions.Unsubscribe();
            this.executionStorage.Dispose();
        }

        private void Subscribe(IConnection connection)
        {
            this.subscriptions = new List<IDisposable>
            {
                connection.Subscribe((AccountValueMessage message) => message.AccountName == this.AccountName,
                    this.OnAccountValueMessage)
            };
        }

        private void OnAccountValueMessage(AccountValueMessage message)
        {
            this.accountCurrenciesFields.Update(AccountValue.FromMessage(message));
            this.AccountChanged(this);
        }
    }
}
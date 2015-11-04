using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using IBApi.Connection;
using IBApi.Executions;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Orders;
using IBApi.Positions;
using CodeContract = System.Diagnostics.Contracts.Contract;

namespace IBApi.Accounts
{
    internal class Account : IAccountInternal
    {
        public Account(string name, IConnection connection, IApiObjectsFactory objectsFactory)
        {
            CodeContract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(name));
            CodeContract.Requires<ArgumentNullException>(connection != null);

            AccountName = name;
            AccountId = name;

            executionStorage = objectsFactory.CreateExecutionStorage(AccountName);
            executionStorage.Initialized += OnChildStorageInitialized;
            positionsStorage = objectsFactory.CreatePositionStorage(AccountName);
            positionsStorage.Initialized += OnChildStorageInitialized;
            ordersStorage = objectsFactory.CreateOrdersStorage(AccountName);
            ordersStorage.Initialized += OnChildStorageInitialized;

            Subscribe(connection);
            SendRequest(connection);
        }

        public event InitializedEventHandler Initialized = delegate { };

        public bool IsInitialized { get; private set; }

        public event AccountChangedEventHandler AccountChanged = delegate { };

        public string AccountName { get; set; }

        public string AccountId { get; set; }

        public AccountFields AccountFields
        {
            get { return accountFields[baseCurrencyId]; }
        }

        public string[] Currencies
        {
            get { return accountFields.Keys.ToArray(); }
        }

        public AccountFields this[string currency]
        {
            get { return accountFields[currency]; }
        }


        public IOrdersStorage OrdersStorage
        {
            get { return ordersStorage; }
        }

        public IExecutionsStorage ExecutionsStorage
        {
            get { return executionStorage; }
        }

        public IPositionsStorage PositionStorage
        {
            get { return positionsStorage; }
        }

        public void Dispose()
        {
            subscriptions.Unsubscribe();
            executionStorage.Dispose();
        }

        private void SendRequest(IConnection connection)
        {
            connection.SendMessage(new RequestAccountUpdatesMessage(AccountName));
        }

        private void Subscribe(IConnection connection)
        {
            subscriptions = new List<IDisposable>
            {
                connection.Subscribe((AccountValueMessage message) => message.AccountName == AccountName,
                    OnAccountValueMessage),
                connection.Subscribe((AccountDownloadEndMessage message) => message.AccountName == AccountName,
                    OnAccountDownloadEndMessage)
            };
        }

        private void OnAccountValueMessage(AccountValueMessage message)
        {
            AccountValue accountValue = AccountValue.FromMessage(message);
            string currency = IsBaseCurrency(accountValue.Currency) ? baseCurrencyId : accountValue.Currency;

            //Trace.TraceInformation("{0} {1} {2}", accountValue.Currency, accountValue.Key, accountValue.Value);

            PropertyInfo property = accountProperties.SingleOrDefault(prop => prop.Name == accountValue.Key);

            if (property == null)
            {
                return;
            }

            AccountFields accountFieldsInstance = GetAccountFieldsInstance(currency);
            property.SetValue(accountFieldsInstance,
                Convert.ChangeType(accountValue.Value, property.PropertyType, CultureInfo.InvariantCulture), null);
            AccountChanged(this);
        }

        private void OnAccountDownloadEndMessage(AccountDownloadEndMessage message)
        {
            Trace.TraceInformation("Downloaded account {0}", message.AccountName);
            positionsStorage.AccountsReceived();
            ordersStorage.AccountsReceived();

            accountDownloaded = true;
            CheckInited();
        }

        private AccountFields GetAccountFieldsInstance(string forCurrency)
        {
            AccountFields accountFieldsInstance;

            if (accountFields.TryGetValue(forCurrency, out accountFieldsInstance))
            {
                return accountFieldsInstance;
            }

            accountFieldsInstance = new AccountFields();
            accountFields[forCurrency] = accountFieldsInstance;
            return accountFieldsInstance;
        }

        private void OnChildStorageInitialized()
        {
            CheckInited();
        }

        private void CheckInited()
        {
            if (!accountDownloaded || !executionStorage.IsInitialized || !positionsStorage.IsInitialized ||
                !ordersStorage.IsInitialized)
            {
                return;
            }

            IsInitialized = true;
            Initialized();
        }

        private static bool IsBaseCurrency(string currency)
        {
            return String.IsNullOrEmpty(currency) || currency == baseCurrencyId;
        }

        private readonly IDictionary<string, AccountFields> accountFields = new Dictionary<string, AccountFields>();
        private readonly IExecutionStorageInternal executionStorage;
        private readonly IOrdersStorageInternal ordersStorage;
        private readonly IPositionsStorageInternal positionsStorage;
        private bool accountDownloaded;
        private ICollection<IDisposable> subscriptions = new List<IDisposable>();
        private const string baseCurrencyId = "BASE";
        private static readonly PropertyInfo[] accountProperties = typeof (AccountFields).GetProperties();
    }
}
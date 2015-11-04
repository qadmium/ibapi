using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IBApi.Connection;
using IBApi.Messages.Client;

namespace IBApi.Accounts
{
    internal sealed class AccountsStorage : IAccountsStorage, IObjectWithDelayedInitialization
    {
        public AccountsStorage(IEnumerable<string> accountsList, IConnection connection, IApiObjectsFactory objectsFactory)
        {
            accounts = accountsList.Select(accountName => CreateAccount(accountName, objectsFactory)).ToList();
            Accounts = accounts.Select<IAccountInternal, IAccount>(account => account).ToList().AsReadOnly();

            SendAutoOpenOrdersRequest(connection);
        }

        public event InitializedEventHandler Initialized;

        public bool IsInitialized { get; private set; }

        public ReadOnlyCollection<IAccount> Accounts
        {
            get; private set;
        }

        public void Dispose()
        {
            foreach (var account in accounts)
            {
                account.Dispose();
            }
        }

        private IAccountInternal CreateAccount(string accountName, IApiObjectsFactory objectsFactory)
        {
            var result = objectsFactory.CreateAccount(accountName);
            result.Initialized += OnAccountInitialized;
            return result;
        }

        private void OnAccountInitialized()
        {
            if (accounts.Any(account => !account.IsInitialized))
            {
                return;
            }

            IsInitialized = true;
            Initialized();
        }

        private void SendAutoOpenOrdersRequest(IConnection connection)
        {
            var request = RequestAutoOpenOrdersMessage.Default;
            connection.SendMessage(request);
        }

        private readonly List<IAccountInternal> accounts;
    }
}

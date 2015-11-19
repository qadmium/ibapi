using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Accounts;
using IBApi.Connection;
using IBApi.Messages.Client;

namespace IBApi.Operations
{
    internal class CreateAccountStorageOperation
    {
        private readonly TaskCompletionSource<IAccountsStorage> taskCompletionSource =
            new TaskCompletionSource<IAccountsStorage>();

        public CreateAccountStorageOperation(CancellationToken cancellationToken, IApiObjectsFactory factory,
            IConnection connection,
            string[] managedAccountsList)
        {
            Contract.Requires(factory != null);
            Contract.Requires(connection != null);
            Contract.Requires(managedAccountsList != null && managedAccountsList.Length > 0);

            SendAutoOpenOrdersRequest(connection);
            this.CreateAndWaitAccounts(managedAccountsList, factory, connection, cancellationToken);
        }

        public Task<IAccountsStorage> Result
        {
            get { return this.taskCompletionSource.Task; }
        }

        private async void CreateAndWaitAccounts(string[] managedAccountsList, IApiObjectsFactory factory,
            IConnection connection, CancellationToken cancellationToken)
        {
            var tasks =
                managedAccountsList.Select(account => factory.CreateAccountOperation(account, cancellationToken))
                    .ToList();

            SendAutoOpenOrdersRequest(connection);

            var accounts = new List<IAccountInternal>();
            foreach (var task in tasks)
            {
                accounts.Add(await task);
            }

            this.taskCompletionSource.SetResult(factory.CreateAccountStorage(accounts));
        }

        private static void SendAutoOpenOrdersRequest(IConnection connection)
        {
            Contract.Requires(connection != null);


            var request = RequestAutoOpenOrdersMessage.Default;
            connection.SendMessage(request);
        }
    }
}
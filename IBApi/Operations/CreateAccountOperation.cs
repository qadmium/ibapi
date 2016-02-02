using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Accounts;
using IBApi.Connection;
using IBApi.Executions;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Orders;
using IBApi.Positions;

namespace IBApi.Operations
{
    internal class CreateAccountOperation
    {
        private readonly string account;
        private readonly AccountCurrenciesFields accountCurrenciesFields = new AccountCurrenciesFields();
        private readonly Task<IExecutionStorageInternal> createExecutionsStorage;
        private readonly IApiObjectsFactory factory;
        private readonly IOrdersStorageInternal ordersStorage;
        private readonly IPositionsStorageInternal positionStorage;

        private readonly TaskCompletionSource<IAccountInternal> taskCompletionSource =
            new TaskCompletionSource<IAccountInternal>();

        private CancellationToken cancellationToken;
        private List<IDisposable> subscriptions;

        public CreateAccountOperation(IConnection connection, IApiObjectsFactory factory, string account,
            CancellationToken cancellationToken)
        {
            Contract.Requires(connection != null);
            Contract.Requires(factory != null);
            Contract.Requires(!string.IsNullOrEmpty(account));

            this.factory = factory;
            this.account = account;
            this.cancellationToken = cancellationToken;
            this.cancellationToken.Register(() => this.taskCompletionSource.TrySetCanceled());
            this.positionStorage = this.factory.CreatePositionStorage(account);
            this.ordersStorage = this.factory.CreateOrdersStorage(account);
            this.createExecutionsStorage = factory.CreateExecutionStorageOperation(account, cancellationToken);
            this.Subscribe(connection);
            this.SendRequest(connection);
        }

        public Task<IAccountInternal> Result
        {
            get { return this.taskCompletionSource.Task; }
        }

        private void SendRequest(IConnection connection)
        {
            Contract.Requires(connection != null);
            connection.SendMessage(new RequestAccountUpdatesMessage(this.account));
        }

        private void Subscribe(IConnection connection)
        {
            Contract.Requires(connection != null);
            this.subscriptions = new List<IDisposable>
            {
                connection.Subscribe((AccountValueMessage message) => message.AccountName == this.account,
                    this.OnAccountValueMessage),
                connection.Subscribe((AccountDownloadEndMessage message) => message.AccountName == this.account,
                    this.OnAccountDownloadEndMessage)
            };
        }

        private async void OnAccountDownloadEndMessage(AccountDownloadEndMessage obj)
        {
            this.subscriptions.Unsubscribe();

            if (this.cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var executionsStorage = await this.createExecutionsStorage;
            this.taskCompletionSource.SetResult(this.factory.CreateAccount(this.account, executionsStorage,
                this.positionStorage, this.ordersStorage, this.accountCurrenciesFields));
        }

        private void OnAccountValueMessage(AccountValueMessage message)
        {
            this.accountCurrenciesFields.Update(AccountValue.FromMessage(message));
        }
    }
}
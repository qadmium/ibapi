using System;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Accounts;
using IBApi.Contracts;
using IBApi.Executions;
using IBApi.Orders;
using IBApi.Positions;

namespace IBApi.Infrastructure
{
    internal class AccountProxy : IAccount
    {
        private readonly Dispatcher dispatcher;
        private readonly IAccountInternal internalAccount;
        private readonly IOrdersStorage ordersStorage;
        private readonly IExecutionsStorage executionsStorage;
        private readonly IPositionsStorage positionsStorage;
        private readonly object accountChangedEvent;

        public AccountProxy(Dispatcher dispatcher, IAccountInternal internalAccount,
            IOrdersStorage ordersStorage, IExecutionsStorage executionsStorage, IPositionsStorage positionsStorage)
        {
            this.dispatcher = dispatcher;
            this.internalAccount = internalAccount;
            this.ordersStorage = ordersStorage;
            this.executionsStorage = executionsStorage;
            this.positionsStorage = positionsStorage;

            this.internalAccount.AccountChanged += this.OnAccountChanged;
            this.accountChangedEvent = this.dispatcher.RegisterEvent();
        }

        private void OnAccountChanged(object sender, AccountChangedEventArgs accountChangedEventArgs)
        {
            this.dispatcher.RaiseEvent(this.accountChangedEvent, this, accountChangedEventArgs);
        }

        public event EventHandler<AccountChangedEventArgs> AccountChanged
        {
            add { this.dispatcher.AddHandler(this.accountChangedEvent, value); }
            remove { this.dispatcher.RemoveHandler(this.accountChangedEvent, value); }
        }

        public string AccountName
        {
            get { return this.dispatcher.Dispatch(() => this.internalAccount.AccountName); }
        }

        public string AccountId
        {
            get { return this.dispatcher.Dispatch(() => this.internalAccount.AccountId); }
        }

        public AccountFields AccountFields
        {
            get { return (AccountFields)this.dispatcher.Dispatch(() => this.internalAccount.AccountFields.Clone()); }
        }

        public string[] Currencies
        {
            get { return this.dispatcher.Dispatch(() => this.internalAccount.Currencies); }
        }

        public AccountFields this[string currency]
        {
            get { return (AccountFields)this.dispatcher.Dispatch(() => this.internalAccount[currency].Clone()); }
        }

        public IOrdersStorage OrdersStorage
        {
            get { return this.ordersStorage; }
        }

        public IExecutionsStorage ExecutionsStorage
        {
            get { return this.executionsStorage; }
        }

        public IPositionsStorage PositionsStorage
        {
            get { return this.positionsStorage; }
        }

        public Task<int> PlaceMarketOrder(Contract contract, int quantity, OrderAction action,
            CancellationToken cancellationToken)
        {
            return this.dispatcher.Dispatch(
                    () => this.internalAccount.PlaceMarketOrder(contract, quantity, action, cancellationToken));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Messages.Server;

namespace IBApi.Positions
{
    internal sealed class PositionsStorage : IPositionsStorageInternal
    {
        private readonly string accountName;

        private readonly IApiObjectsFactory objectsFactory;
        private readonly IDictionary<Contract, Position> positions = new Dictionary<Contract, Position>();
        private IDisposable subscription;

        public PositionsStorage(IConnection connection, IApiObjectsFactory objectsFactory, string accountName)
        {
            System.Diagnostics.Contracts.Contract.Requires(connection != null);
            System.Diagnostics.Contracts.Contract.Requires(objectsFactory != null);
            System.Diagnostics.Contracts.Contract.Requires(!string.IsNullOrEmpty(accountName));
            this.objectsFactory = objectsFactory;
            this.accountName = accountName;
            this.Subscribe(connection);
        }

        public event PositionAddedEventHandler PositionAdded = delegate { };

        public ReadOnlyCollection<IPosition> Positions
        {
            get
            {
                var castedPositions = this.positions.Values.Select<Position, IPosition>(position => position).ToList();
                return castedPositions.AsReadOnly();
            }
        }

        public void Dispose()
        {
            this.subscription.Dispose();
        }

        private void Subscribe(IConnection connection)
        {
            System.Diagnostics.Contracts.Contract.Requires(connection != null);
            this.subscription =
                connection.Subscribe((PortfolioValueMessage message) => message.AccountName == this.accountName,
                    this.OnPositionUpdate);
        }

        private void OnPositionUpdate(PortfolioValueMessage message)
        {
            var positionContract = Contract.FromPortfolioValueMessage(message);

            Position position;

            if (this.positions.TryGetValue(positionContract, out position))
            {
                position.Update(message, this.accountName);
                return;
            }

            position = this.objectsFactory.CreatePosition();
            position.Update(message, this.accountName);
            this.positions[positionContract] = position;
            this.PositionAdded(position);
        }
    }
}
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
        public event PositionAddedEventHandler PositionAdded = delegate { };

        public ReadOnlyCollection<IPosition> Positions
        {
            get
            {
                var castedPositions = positions.Values.Select<Position, IPosition>(position => position).ToList();
                return new ReadOnlyCollection<IPosition>(castedPositions);
            }
        }
        
        public event InitializedEventHandler Initialized = delegate { };
        
        public bool IsInitialized { get; private set; }

        public PositionsStorage(IConnection connection, IApiObjectsFactory objectsFactory, string accountName)
        {
            this.objectsFactory = objectsFactory;
            this.accountName = accountName;

            Subscribe(connection);
        }

        public void AccountsReceived()
        {
            IsInitialized = true;
            Initialized();
        }

        public void Dispose()
        {
            subscription.Dispose();
        }

        private void Subscribe(IConnection connection)
        {
            subscription = connection.Subscribe((PortfolioValueMessage message) => message.AccountName == accountName,
                OnPositionUpdate);
        }

        private void OnPositionUpdate(PortfolioValueMessage message)
        {
            var positionContract = Contract.FromPortfolioValueMessage(message);

            Position position;

            if (positions.TryGetValue(positionContract, out position))
            {
                position.Update(message);  
                return;
            }

            position = objectsFactory.CreatePosition();
            position.Update(message);
            positions[positionContract] = position;
            PositionAdded(position);
        }

        private readonly IApiObjectsFactory objectsFactory;
        private readonly string accountName;
        private IDisposable subscription;
        private readonly IDictionary<Contract, Position> positions = new Dictionary<Contract, Position>();
    }
}

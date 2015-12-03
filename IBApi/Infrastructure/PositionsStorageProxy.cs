using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using IBApi.Positions;

namespace IBApi.Infrastructure
{
    internal class PositionsStorageProxy : IPositionsStorage
    {
        private readonly Dispatcher dispatcher;
        private readonly ProxiesFactory proxiesFactory;
        private readonly object positionAddedEvent;
        private readonly List<IPosition> positions;

        public PositionsStorageProxy(IPositionsStorage positionsStorage, Dispatcher dispatcher, ProxiesFactory proxiesFactory)
        {
            Contract.Requires(positionsStorage != null);
            Contract.Requires(dispatcher != null);
            Contract.Requires(proxiesFactory != null);

            this.dispatcher = dispatcher;
            this.proxiesFactory = proxiesFactory;

            this.positionAddedEvent = dispatcher.RegisterEvent();
            positionsStorage.PositionAdded += this.OnPositionAdded;
            this.positions = positionsStorage.Positions.Select(proxiesFactory.CreatePositionProxy).ToList();
        }

        private void OnPositionAdded(object sender, PositionAddedEventArgs positionAddedEventArgs)
        {
            var proxy = this.proxiesFactory.CreatePositionProxy(positionAddedEventArgs.Position);
            this.positions.Add(proxy);

            this.dispatcher.RaiseEvent(this.positionAddedEvent, this, new PositionAddedEventArgs{Position = proxy});
        }

        public event EventHandler<PositionAddedEventArgs> PositionAdded
        {
            add { this.dispatcher.AddHandler(this.positionAddedEvent, value); }
            remove { this.dispatcher.RemoveHandler(this.positionAddedEvent, value); }
        }

        public IReadOnlyCollection<IPosition> Positions
        {
            get { return this.dispatcher.Dispatch(() => this.positions.ToList().AsReadOnly()); }
        }
    }
}
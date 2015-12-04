using System;
using IBApi.Positions;

namespace IBApi.Infrastructure
{
    internal class PositionsProxy : IPosition
    {
        private readonly IPosition position;
        private readonly Dispatcher dispatcher;
        private readonly object positionChangedEvent;

        public PositionsProxy(IPosition position, Dispatcher dispatcher)
        {
            System.Diagnostics.Contracts.Contract.Requires(position != null);
            System.Diagnostics.Contracts.Contract.Requires(dispatcher != null);

            this.position = position;
            this.dispatcher = dispatcher;

            this.positionChangedEvent = this.dispatcher.RegisterEvent();
            position.PositionChanged += this.OnPositionChanged;
        }

        private void OnPositionChanged(object sender, PositionChangedEventArgs positionChangedEventArgs)
        {
            this.dispatcher.RaiseEvent(this.positionChangedEvent, this, new PositionChangedEventArgs { Position = this });
        }

        public event EventHandler<PositionChangedEventArgs> PositionChanged
        {
            add { this.dispatcher.AddHandler(this.positionChangedEvent, value); }
            remove { this.dispatcher.RemoveHandler(this.positionChangedEvent, value); }
        }

        public Contracts.Contract Contract
        {
            get { return this.dispatcher.Dispatch(() => this.position.Contract); }
        }

        public string AccountName
        {
            get { return this.dispatcher.Dispatch(() => this.position.AccountName); }
        }

        public double AveragePrice
        {
            get { return this.dispatcher.Dispatch(() => this.position.AveragePrice); }
        }

        public double MarketPrice
        {
            get { return this.dispatcher.Dispatch(() => this.position.MarketPrice); }
        }

        public double MarketValue
        {
            get { return this.dispatcher.Dispatch(() => this.position.MarketValue); }
        }

        public int Quantity
        {
            get { return this.dispatcher.Dispatch(() => this.position.Quantity); }
        }

        public double RealizedPL
        {
            get { return this.dispatcher.Dispatch(() => this.position.RealizedPL); }
        }

        public double OpenPL
        {
            get { return this.dispatcher.Dispatch(() => this.position.OpenPL); }
        }
    }
}
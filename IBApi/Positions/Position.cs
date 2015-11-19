using System;
using IBApi.Contracts;
using IBApi.Messages.Server;

namespace IBApi.Positions
{
    internal sealed class Position : IPosition, IDisposable
    {
        public void Dispose()
        {
        }

        public event PositionChangedEventHandler PositionChanged = delegate { };

        public Contract Contract { get; private set; }
        public string AccountName { get; private set; }

        public double AveragePrice { get; private set; }

        public double MarketPrice { get; private set; }

        public double MarketValue { get; private set; }

        public int Quantity { get; private set; }

        public double RealizedPL { get; private set; }

        public double OpenPL { get; private set; }

        public void Update(PortfolioValueMessage message, string accountName)
        {
            this.Contract = Contract.FromPortfolioValueMessage(message);
            this.AveragePrice = message.AverageCost;
            this.MarketPrice = message.MarketPrice;
            this.MarketValue = message.MarketValue;
            this.Quantity = message.Position;
            this.RealizedPL = message.RealizedPNL;
            this.OpenPL = message.UnrealizedPNL;
            this.AccountName = accountName;

            this.PositionChanged(this);
        }
    }
}
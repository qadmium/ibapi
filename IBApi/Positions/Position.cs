using System;
using IBApi.Contracts;
using IBApi.Messages.Server;

namespace IBApi.Positions
{
    internal sealed class Position : IPosition, IDisposable
    {
        public event PositionChangedEventHandler PositionChanged = delegate { };
        
        public Contract Contract { get; private set; }
        
        public double AveragePrice { get; private set; }
        
        public double MarketPrice { get; private set; }
        
        public double MarketValue { get; private set; }
        
        public int Quantity { get; private set; }
        
        public double RealizedPL { get; private set; }
        
        public double OpenPL { get; private set; }

        public void Update(PortfolioValueMessage message)
        {
            Contract = Contract.FromPortfolioValueMessage(message);
            AveragePrice = message.AverageCost;
            MarketPrice = message.MarketPrice;
            MarketValue = message.MarketValue;
            Quantity = message.Position;
            RealizedPL = message.RealizedPNL;
            OpenPL = message.UnrealizedPNL;

            PositionChanged(this);
        }

        public void Dispose()
        {
        }
    }
}

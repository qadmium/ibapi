using System;
using IBApi.Contracts;

namespace IBApi.Positions
{
    public class PositionChangedEventArgs : EventArgs
    {
        public IPosition Position { get; internal set; }
    }

    public interface IPosition
    {
        event EventHandler<PositionChangedEventArgs> PositionChanged;

        Contract Contract { get; }
        string AccountName { get; }
        double AveragePrice { get; }
        double MarketPrice { get; }
        double MarketValue { get; }
        int Quantity { get; }
        double RealizedPL { get; }
        double OpenPL { get; }
    }
}
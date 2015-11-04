using IBApi.Contracts;

namespace IBApi.Positions
{
    public delegate void PositionChangedEventHandler(IPosition account);

    public interface IPosition
    {
        event PositionChangedEventHandler PositionChanged;

        Contract Contract { get; }

        double AveragePrice { get; }
        double MarketPrice { get; }
        double MarketValue { get; }
        int Quantity { get; }
        double RealizedPL { get; }
        double OpenPL { get; }
    }
}
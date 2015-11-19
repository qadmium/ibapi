using IBApi.Contracts;

namespace IBApi.Positions
{
    public delegate void PositionChangedEventHandler(IPosition position);

    public interface IPosition
    {
        event PositionChangedEventHandler PositionChanged;

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
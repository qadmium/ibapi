using System.Collections.ObjectModel;

namespace IBApi.Positions
{
    public delegate void PositionAddedEventHandler(IPosition position);

    public interface IPositionsStorage
    {
        event PositionAddedEventHandler PositionAdded;

        ReadOnlyCollection<IPosition> Positions { get; }
    }
}
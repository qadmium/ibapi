using System;
using System.Collections.Generic;

namespace IBApi.Positions
{
    public class PositionAddedEventArgs : EventArgs
    {
        public IPosition Position { get; set; }
    }
    public interface IPositionsStorage
    {
        event EventHandler<PositionAddedEventArgs> PositionAdded;

        IReadOnlyCollection<IPosition> Positions { get; }
    }
}
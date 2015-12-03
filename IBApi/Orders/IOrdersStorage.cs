using System;
using System.Collections.Generic;

namespace IBApi.Orders
{
    public class OrderAddedEventArgs : EventArgs
    {
        public IOrder Order { get; internal set; }
    }

    public interface IOrdersStorage
    {
        event EventHandler<OrderAddedEventArgs> OrderAdded;

        IReadOnlyCollection<IOrder> Orders { get; }  
    }
}
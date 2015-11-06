using System.Collections.Generic;

namespace IBApi.Orders
{
    public delegate void OrderAddedEventHandler(IOrder position);

    public interface IOrdersStorage
    {
        event OrderAddedEventHandler OrderAdded;

        IReadOnlyCollection<IOrder> Orders { get; }  
    }
}
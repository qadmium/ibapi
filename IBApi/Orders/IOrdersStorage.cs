using System.Collections.Generic;

namespace IBApi.Orders
{
    public delegate void OrderAddedEventHandler(IOrder order);

    public interface IOrdersStorage
    {
        event OrderAddedEventHandler OrderAdded;

        IReadOnlyCollection<IOrder> Orders { get; }  
    }
}
using System.Collections.ObjectModel;

namespace IBApi.Orders
{
    public delegate void OrderAddedEventHandler(IOrder position);

    public interface IOrdersStorage
    {
        event OrderAddedEventHandler OrderAdded;

        ReadOnlyCollection<IOrder> Orders { get; }  
    }
}
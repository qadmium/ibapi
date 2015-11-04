using System;

namespace IBApi.Orders
{
    internal interface IOrdersStorageInternal : IOrdersStorage, IObjectWithDelayedInitialization, IDisposable
    {
        void AccountsReceived();
    }
}
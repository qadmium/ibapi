using System;

namespace IBApi.Positions
{
    internal interface IPositionsStorageInternal : IPositionsStorage, IObjectWithDelayedInitialization, IDisposable
    {
        void AccountsReceived();
    }
}
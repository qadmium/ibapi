using System;
using IBApi.Messages.Server;

namespace IBApi.Connection
{
    internal interface ISubscription : IDisposable
    {
        void OnMessage(IServerMessage message) ;
    }
}
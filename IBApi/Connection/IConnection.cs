using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using IBApi.Messages.Client;

namespace IBApi.Connection
{
    [ContractClass(typeof(IConnectionContract))]
    internal interface IConnection : IDisposable
    {
        void Run();
        bool Running { get;}
        void SendMessage(IClientMessage message);
        int NextRequestId();

        IDisposable Subscribe<T>(Func<T, bool> condition, Action<T> callback, TaskScheduler scheduler);
        IDisposable Subscribe<T>(Func<T, bool> condition, Action<T> callback);
        IDisposable Subscribe<T>(Action<T> callback);
    }
}
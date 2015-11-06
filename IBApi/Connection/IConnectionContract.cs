using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using IBApi.Messages.Client;

namespace IBApi.Connection
{
    // ReSharper disable once InconsistentNaming
    [ContractClassFor(typeof (IConnection))]
    [DebuggerStepThrough]
    internal abstract class IConnectionContract : IConnection
    {
        public abstract void Dispose();

        public void SendMessage(IClientMessage message)
        {
            Contract.Requires(message != null);
        }

        public abstract int NextRequestId();

        public IDisposable Subscribe<T>(Func<T, bool> condition, Action<T> callback)
        {
            Contract.Requires(condition != null);
            Contract.Requires(callback != null);
            return null;
        }

        public IDisposable Subscribe<T>(Action<T> callback)
        {
            Contract.Requires(callback != null);
            return null;
        }
    }
}
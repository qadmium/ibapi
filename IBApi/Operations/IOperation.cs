using System;
using IBApi.Connection;
using IBApi.Errors;

namespace IBApi.Operations
{
    internal delegate void OperationCompletedEventHandler();
    internal delegate void OperationFailedEventHandler(Error error);

    internal interface IOperation : IDisposable
    {
        event OperationCompletedEventHandler OperationCompleted;
        event OperationFailedEventHandler OperationFailed;
        bool Failed { get; }
        bool Completed { get; }
        void Execute(IConnection connection);
    }
}
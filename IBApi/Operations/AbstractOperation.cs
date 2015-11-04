using System.Diagnostics.Contracts;
using IBApi.Connection;
using IBApi.Errors;

namespace IBApi.Operations
{
    [ContractClass(typeof(AbstractOperationContract))]
    internal abstract class AbstractOperation : IOperation
    {
        public event OperationCompletedEventHandler OperationCompleted = delegate { };
        public event OperationFailedEventHandler OperationFailed = delegate { };

        public bool Failed { get; private set; }
        public bool Completed { get; private set; }

        public abstract void Execute(IConnection connection);
        public abstract void Dispose();

        protected virtual void OnOperationFailed(Error error)
        {
            Failed = true;
            OperationFailed(error);
        }

        protected virtual void OnOperationCompleted()
        {
            Completed = true;
            OperationCompleted();
        }
    }
}
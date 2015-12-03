using System;
using System.Collections.Generic;

namespace IBApi.Executions
{
    public sealed class ExecutionAddedEventArgs : EventArgs
    {
        public Execution Execution { get; set; }
    }

    public interface IExecutionsStorage
    {
        event EventHandler<ExecutionAddedEventArgs> ExecutionAdded;

        IReadOnlyCollection<Execution> Executions { get; } 
    }
}
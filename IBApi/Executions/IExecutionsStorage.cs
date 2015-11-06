using System.Collections.Generic;

namespace IBApi.Executions
{
    public delegate void ExecutionAddedEventHandler(Execution execution);

    public interface IExecutionsStorage
    {
        event ExecutionAddedEventHandler ExecutionAdded;

        IReadOnlyCollection<Execution> Executions { get; } 
    }
}
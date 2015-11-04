using System.Collections.ObjectModel;

namespace IBApi.Executions
{
    public delegate void ExecutionAddedEventHandler(Execution execution);

    public interface IExecutionsStorage
    {
        event ExecutionAddedEventHandler ExecutionAdded;

        ReadOnlyCollection<Execution> Executions { get; } 
    }
}
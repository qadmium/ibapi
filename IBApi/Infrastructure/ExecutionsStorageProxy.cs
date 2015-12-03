using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IBApi.Executions;

namespace IBApi.Infrastructure
{
    internal sealed class ExecutionsStorageProxy : IExecutionsStorage
    {
        private readonly Dispatcher dispatcher;
        private readonly object executionAddedEvent;
        private readonly IExecutionsStorage executionsStorage;

        public ExecutionsStorageProxy(IExecutionsStorage executionsStorage, Dispatcher dispatcher)
        {
            this.executionsStorage = executionsStorage;
            this.dispatcher = dispatcher;
            this.executionsStorage.ExecutionAdded += this.OnExecutionAdded;
            this.executionAddedEvent = this.dispatcher.RegisterEvent();
        }

        public event EventHandler<ExecutionAddedEventArgs> ExecutionAdded
        {
            add { this.dispatcher.AddHandler(this.executionAddedEvent, value); }
            remove { this.dispatcher.RemoveHandler(this.executionAddedEvent, value); }
        }

        public IReadOnlyCollection<Execution> Executions
        {
            get
            {
                return
                    new ReadOnlyCollection<Execution>(
                        this.dispatcher.Dispatch(() => this.executionsStorage.Executions.ToList()));
            }
        }

        private void OnExecutionAdded(object sender, ExecutionAddedEventArgs executionAddedEventArgs)
        {
            this.dispatcher.RaiseEvent(executionAddedEventArgs, this, executionAddedEventArgs);
        }
    }
}
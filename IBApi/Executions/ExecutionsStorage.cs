using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using IBApi.Connection;
using IBApi.Messages.Server;

namespace IBApi.Executions
{
    internal sealed class ExecutionsStorage : IExecutionStorageInternal
    {
        private readonly string accountName;

        private readonly IConnection connection;
        private readonly List<Execution> executions;
        private readonly IList<IDisposable> subscriptions = new List<IDisposable>();

        public ExecutionsStorage(IConnection connection, string accountName, List<Execution> executions)
        {
            Contract.Requires(connection != null);
            Contract.Requires(accountName != null);
            Contract.Requires(executions != null);

            this.executions = executions;
            this.connection = connection;
            this.accountName = accountName;
            this.Subscribe();
        }

        public event ExecutionAddedEventHandler ExecutionAdded = delegate { };

        public IReadOnlyCollection<Execution> Executions
        {
            get { return this.executions.AsReadOnly(); }
        }

        public void Dispose()
        {
            this.subscriptions.Unsubscribe();
        }

        private void Subscribe()
        {
            this.subscriptions.Add(
                this.connection.Subscribe((ExecutionDataMessage message) => message.Account == this.accountName,
                    this.OnExecutionData));
        }

        private void OnExecutionData(ExecutionDataMessage message)
        {
            var execution = Execution.FromMessage(message, this.accountName);
            this.executions.Add(execution);
            this.ExecutionAdded(execution);
        }
    }
}
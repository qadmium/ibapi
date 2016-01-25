using System.Linq;
using Caliburn.Micro;
using IBApi;
using IBApi.Executions;

namespace Sample.Executions
{
    class ExecutionsViewModel : Screen
    {
        private IObservableCollection<ExecutionView> executions;

        public ExecutionsViewModel(IClient client)
        {
            this.DisplayName = "Executions";

            this.executions = new BindableCollection<ExecutionView>();

            foreach (var execution in client.Accounts.SelectMany(account => account.ExecutionsStorage.Executions))
            {
                this.executions.Add(new ExecutionView(execution));
            }

            foreach (var account in client.Accounts)
            {
                account.ExecutionsStorage.ExecutionAdded += this.OnExecutionAdded;
            }
        }

        public IObservableCollection<ExecutionView> Executions
        {
            get { return this.executions; }
            set
            {
                if (Equals(value, this.executions)) return;
                this.executions = value;
                this.NotifyOfPropertyChange(() => this.Executions);
            }
        }

        private void OnExecutionAdded(object sender, ExecutionAddedEventArgs executionAddedEventArgs)
        {
            this.Executions.Add(new ExecutionView(executionAddedEventArgs.Execution));
        }
    }
}

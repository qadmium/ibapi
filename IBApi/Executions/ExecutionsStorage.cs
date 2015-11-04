using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using IBApi.Connection;
using IBApi.Contracts;
using IBApi.Messages.Client;
using IBApi.Messages.Server;

namespace IBApi.Executions
{
    internal sealed class ExecutionsStorage : IExecutionStorageInternal
    {
        public event InitializedEventHandler Initialized = delegate { };
        
        public bool IsInitialized { get; private set; }

        public event ExecutionAddedEventHandler ExecutionAdded = delegate { };

        public ReadOnlyCollection<Execution> Executions
        {
            get
            {
                return new ReadOnlyCollection<Execution>(executions);
            }
        }

        public ExecutionsStorage(IConnection connection, string accountName)
        {
            this.connection = connection;
            this.accountName = accountName;
            requestId = connection.NextRequestId();

            Subscribe();
            SendRequest();
        }

        public void Dispose()
        {
            subscriptions.Unsubscribe();
        }

        private void SendRequest()
        {
            var request = RequestExecutionsMessage.Default;
            request.FilterByAccountCode = accountName;

            connection.SendMessage(request);
        }

        private void Subscribe()
        {
            subscriptions = new List<IDisposable>
            {
                connection.Subscribe((ExecutionDataMessage message) => message.RequestId == requestId, OnExecutionData),
                connection.Subscribe((ExecutionDataEndMessage message) => message.RequestId == requestId,
                    OnExecutionDataEnd),
            };
        }

        private void OnExecutionData(ExecutionDataMessage message)
        {
            var executionTime = DateTime.ParseExact(message.ExecutionTime, "yyyyMMdd  HH:mm:ss", CultureInfo.InvariantCulture);

            var execution = new Execution
            {
                Account = accountName,
                Buy = message.Side == "BOT",
                Exchange = message.ExecutionExchange,
                Id = message.ExecutionId,
                OrderId = message.OrderId,
                Price =  message.Price,
                AveragePrice = message.AveragePrice,
                CumQuantity = message.CumQty,
                Quantity = message.Shares,
                Time = executionTime,
                Contract = Contract.FromExecutionDataMessage(message)
            };

            executions.Add(execution);
            ExecutionAdded(execution);
        }

        private void OnExecutionDataEnd(ExecutionDataEndMessage dataEndMessage)
        {
            IsInitialized = true;
            Initialized();

            subscriptions.Unsubscribe();
            subscriptions.Add(connection.Subscribe((ExecutionDataMessage message) => message.Account == accountName, OnExecutionData));
        }

        private readonly IConnection connection;
        private readonly int requestId;
        private readonly IList<Execution> executions = new List<Execution>();
        private ICollection<IDisposable> subscriptions;
        private readonly string accountName;
    }
}

using System.Collections.Generic;
using IBApi.Executions;
using IBApi.Messages.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class ExecutionsStorageTests
    {
        private ConnectionHelper connectionHelper;

        [TestInitialize]
        public void Init()
        {
            this.connectionHelper = new ConnectionHelper();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureThatStorageCreatesExecutionsOnExecutionDataMessage()
        {
            var executionsStorage = new ExecutionsStorage(this.connectionHelper.Connection(), "testaccount", new List<Execution>());

            this.connectionHelper.SendMessage(new ExecutionDataMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Account = "testaccount",
                ExecutionTime = "20140507  14:00:00",
                SecurityType = "STK"
            });

            this.connectionHelper.SendMessage(new ExecutionDataEndMessage {RequestId = ConnectionHelper.RequestId});

            this.connectionHelper.SendMessage(new ExecutionDataMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Account = "testaccount",
                ExecutionTime = "20140507  14:00:00",
                SecurityType = "STK"
            });

            Assert.AreEqual(2, executionsStorage.Executions.Count);

            executionsStorage.Dispose();
        }
    }
}
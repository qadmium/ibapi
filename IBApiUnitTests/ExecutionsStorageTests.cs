using IBApi;
using IBApi.Executions;
using IBApi.Messages.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class ExecutionsStorageTests
    {
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();
        }

        [TestCleanup]
        public void Cleanup()
        {
            connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureWhatExecutionsStorageInitializedOnExecutionDataEndMessage()
        {
            var executionsStorage = new ExecutionsStorage(connectionHelper.Connection(), "testaccount");

            var initilizationCallback = new Mock<InitializedEventHandler>();
            executionsStorage.Initialized += initilizationCallback.Object;

            Assert.IsFalse(executionsStorage.IsInitialized);
            initilizationCallback.Verify(callback => callback(), Times.Never);

            connectionHelper.SendMessage(new ExecutionDataEndMessage {RequestId = ConnectionHelper.RequestId});

            Assert.IsTrue(executionsStorage.IsInitialized);
            initilizationCallback.Verify(callback => callback(), Times.Once);

            executionsStorage.Dispose();
        }

        [TestMethod]
        public void EnsureWhatStorageCreatesExecutionsOnExecutionDataMessage()
        {
            var executionsStorage = new ExecutionsStorage(connectionHelper.Connection(), "testaccount");

            connectionHelper.SendMessage(new ExecutionDataMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Account = "testaccount",
                ExecutionTime = "20140507  14:00:00",
                SecurityType = "STK"
            });

            connectionHelper.SendMessage(new ExecutionDataEndMessage { RequestId = ConnectionHelper.RequestId });

            connectionHelper.SendMessage(new ExecutionDataMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Account = "testaccount",
                ExecutionTime = "20140507  14:00:00",
                SecurityType = "STK"
            });

            Assert.AreEqual(2, executionsStorage.Executions.Count);

            executionsStorage.Dispose();
        }

        private ConnectionHelper connectionHelper;
    }
}

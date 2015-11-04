using IBApi.Errors;
using IBApi.Messages.Server;
using IBApi.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class WaitForMarketConnectedOperationTests
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
        public void EnsureThatOperationCompletedOnMarketConnected()
        {
            var operation = new WaitForMarketConnectedOperation();
            operation.Execute(connectionHelper.Connection());

            connectionHelper.SendMessage(new ErrorMessage{ErrorCode = ErrorCode.MarketFarmConnected, Message = "Connected"});

            Assert.IsTrue(operation.Completed);
            Assert.IsFalse(operation.Failed);
            operation.Dispose();
        }

        [TestMethod]
        public void EnsureThatOperationFailedOnMarketConnected()
        {
            var operation = new WaitForMarketConnectedOperation();
            operation.Execute(connectionHelper.Connection());

            connectionHelper.SendMessage(new ErrorMessage { ErrorCode = ErrorCode.UnknownError, Message = "Fail" });

            Assert.IsFalse(operation.Completed);
            Assert.IsTrue(operation.Failed);

            operation.Dispose();
        }

        private ConnectionHelper connectionHelper;
    }
}

using IBApi.Messages.Server;
using IBApi.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class ReceiveManagedAccountsListOperationTests
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
        public void EnsureThatOperationCompletedOnAccountListReceived()
        {
            var operation = new ReceiveManagedAccountsListOperation();
            operation.Execute(connectionHelper.Connection());

            connectionHelper.SendMessage(new ManagedAccountsListMessage{AccountsList = "Acc1,Acc2,Acc3"});

            Assert.IsTrue(operation.Completed);
            CollectionAssert.AreEqual(new[] { "Acc1", "Acc2", "Acc3" }, operation.AccountsList);

            operation.Dispose();
        }

        private ConnectionHelper connectionHelper;
    }
}

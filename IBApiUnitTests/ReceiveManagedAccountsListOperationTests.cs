using System.Threading;
using System.Threading.Tasks;
using IBApi.Messages.Server;
using IBApi.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class ReceiveManagedAccountsListOperationTests
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
        public async Task EnsureThatOperationCompletedOnAccountListReceived()
        {
            var operation = new ReceiveManagedAccountsListOperation(this.connectionHelper.Connection(), CancellationToken.None);

            this.connectionHelper.SendMessage(new ManagedAccountsListMessage {AccountsList = "Acc1,Acc2,Acc3"});

            var result = await operation.Result;

            CollectionAssert.AreEqual(new[] {"Acc1", "Acc2", "Acc3"}, result);
        }
    }
}
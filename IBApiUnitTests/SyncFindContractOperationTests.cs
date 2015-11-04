using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Contracts;
using IBApi.Messages.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class SyncFindContractOperationTests
    {
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();
            syncFindContractOperation = new SyncFindContractOperation(connectionHelper.Connection());
        }

        [TestCleanup]
        public void Cleanup()
        {
            syncFindContractOperation.Dispose();
            connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureWhatContractWillBeReturnedOnValidMessage()
        {
            var request = new SearchRequest();

            var result = Task.Run(() => syncFindContractOperation.ResultFor(request, 10000));
            WaitForTaskRunning(result);

            var message = new ContractDataMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Symbol = "MSFT",
                SecurityType = "STK"
            };
            
            connectionHelper.SendMessage(message);
            Assert.AreEqual(Contract.FromContractDataMessage(message), result.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(ContractSearchTimeoutException))]
        public void EnsureWhatExceptionThrownOnTimeout()
        {
            var request = new SearchRequest();

            var result = Task.Run(() => syncFindContractOperation.ResultFor(request, 10));
            WaitForTaskRunning(result);

            throw result.Exception.InnerExceptions.First();
        }

        [TestMethod]
        [ExpectedException(typeof(ContractSearchException), "Nothing found")]
        public void EnsureWhatExceptionWillBeThrownOnNothingFound()
        {
            var request = new SearchRequest();

            var result = Task.Run(() => syncFindContractOperation.ResultFor(request, 1000));
            WaitForTaskRunning(result);

            var message = new ContractDataEndMessage
            {
                RequestId = ConnectionHelper.RequestId,
            };

            connectionHelper.SendMessage(message);

            Delay();

            throw result.Exception.InnerExceptions.First();
        }

        [TestMethod]
        [ExpectedException(typeof(ContractSearchException), "Test Error")]
        public void EnsureWhatExceptionWillBeThrownOnErrorMessage()
        {
            var request = new SearchRequest();

            var result = Task.Run(() => syncFindContractOperation.ResultFor(request, 1000));
            WaitForTaskRunning(result);

            var message = new ErrorMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Message = "Test Error"
            };

            connectionHelper.SendMessage(message);

            Delay();

            throw result.Exception.InnerExceptions.First();
        }

        private static void Delay()
        {
            Thread.Sleep(100);
        }

        private void WaitForTaskRunning(Task task)
        {
            while (task.Status != TaskStatus.Running){}
            Delay();
        }

        private ConnectionHelper connectionHelper;
        private SyncFindContractOperation syncFindContractOperation;
    }
}

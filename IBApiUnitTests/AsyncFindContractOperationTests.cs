using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Contracts;
using IBApi.Errors;
using IBApi.Exceptions;
using IBApi.Messages.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class AsyncFindContractOperationTests
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
        public async Task EnsureThatObserverReceivesErrorOnErrorMessage()
        {
            var task = this.CreateOperation(new SearchRequest());

            var error = new ErrorMessage
            {
                ErrorCode = ErrorCode.DataFarmConnected,
                Message = "123",
                RequestId = ConnectionHelper.RequestId
            };

            this.connectionHelper.SendMessage(error);

            try
            {
                await task;
            }
            catch (IbException exception)
            {
                Assert.AreEqual(error.ErrorCode, exception.ErrorCode);
                Assert.AreEqual(error.Message, exception.Message);
                return;
            }

            Assert.Fail("Exception was expected");
        }

        [TestMethod]
        public async Task EnsureThatObserverReceivesCompletedOnContractDataEndMessage()
        {
            var task = this.CreateOperation(new SearchRequest());

            var message = new ContractDataEndMessage
            {
                RequestId = ConnectionHelper.RequestId
            };

            this.connectionHelper.SendMessage(message);

            var result = await task;
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task EnsureThatObserverReceivesContractOnContractDataMessage()
        {
            var task = this.CreateOperation(new SearchRequest());

            this.connectionHelper.SendMessage(new ContractDataMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Symbol = "MSFT",
                SecurityType = "STK"
            });

            this.connectionHelper.SendMessage(new ContractDataEndMessage
            {
                RequestId = ConnectionHelper.RequestId,
            });

            var result = await task;
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(Contract.FromContractDataMessage(new ContractDataMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Symbol = "MSFT",
                SecurityType = "STK"
            }), result.First());
        }

        private Task<IReadOnlyCollection<Contract>> CreateOperation(SearchRequest request)
        {
            var result = new FindContractsOperation(this.connectionHelper.Connection(), 
                this.connectionHelper.Dispenser(),
                request, CancellationToken.None);
            return result.Task;
        }
    }
}
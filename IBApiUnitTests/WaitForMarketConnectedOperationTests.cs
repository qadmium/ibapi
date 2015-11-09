using System;
using System.Threading;
using System.Threading.Tasks;
using IBApi.Errors;
using IBApi.Exceptions;
using IBApi.Messages.Server;
using IBApi.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class WaitForMarketConnectedOperationTests
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
        public async Task EnsureThatOperationCompletedOnMarketConnected()
        {
            var operation = new WaitForMarketConnectedOperation(this.connectionHelper.Connection(), CancellationToken.None);

            this.connectionHelper.SendMessage(new ErrorMessage
            {
                ErrorCode = ErrorCode.MarketFarmConnected,
                Message = "Connected"
            });

            await operation.Result;
        }

        [TestMethod]
        public async Task EnsureThatOperationFailedOnMarketConnected()
        {
            var operation = new WaitForMarketConnectedOperation(this.connectionHelper.Connection(), CancellationToken.None);

            this.connectionHelper.SendMessage(new ErrorMessage {ErrorCode = ErrorCode.UnknownError, Message = "Fail"});

            try
            {
                await operation.Result;
            }
            catch (IBException exception)
            {
                Assert.AreEqual(ErrorCode.UnknownError, exception.ErrorCode);
                Assert.AreEqual("Fail", exception.Message);
                return;
            }

            Assert.Fail("Execption expected but not trown");
        }
    }
}
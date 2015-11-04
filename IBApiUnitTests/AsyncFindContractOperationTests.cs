using System;
using IBApi.Contracts;
using IBApi.Errors;
using IBApi.Messages.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class AsyncFindContractOperationTests
    {
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();
            observerMock = new Mock<IObserver<Contract>>();
            operation = CreateOperation();
        }

        [TestCleanup]
        public void Cleanup()
        {
            operation.Dispose();
            connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureWhatObserverReceivesErrorOnErrorMessage()
        {
            var error = new ErrorMessage
            {
                ErrorCode = ErrorCode.DataFarmConnected,
                Message = "123",
                RequestId = ConnectionHelper.RequestId
            };

            connectionHelper.SendMessage(error);

            var expectedError = new Error
            {
               Code = error.ErrorCode,
               Message = error.Message,
               RequestId = error.RequestId
            };

            observerMock.Verify(observer => observer.OnError(new ContractSearchException(expectedError)), Times.Once);
        }

        [TestMethod]
        public void EnsureWhatObserverReceivesCompletedOnContractDataEndMessage()
        {
            var message = new ContractDataEndMessage
            {
                RequestId = ConnectionHelper.RequestId
            };

            connectionHelper.SendMessage(message);

            observerMock.Verify(observer => observer.OnCompleted(), Times.Once);
        }

        [TestMethod]
        public void EnsureWhatObserverReceivesContractOnContractDataMessage()
        {
            var message = new ContractDataMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Symbol = "MSFT",
                SecurityType = "STK"
            };

            connectionHelper.SendMessage(message);

            observerMock.Verify(observer => observer.OnNext(Contract.FromContractDataMessage(message)), Times.Once);
        }

        private AsyncFindContractOperation CreateOperation()
        {
            var result = new AsyncFindContractOperation(connectionHelper.Connection());

            result.Start(observerMock.Object, new SearchRequest { SecurityType = SecurityType.STK });
            return result;
        }

        private AsyncFindContractOperation operation;
        private ConnectionHelper connectionHelper;
        private Mock<IObserver<Contract>> observerMock;
    }
}

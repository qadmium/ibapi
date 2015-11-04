using IBApi;
using IBApi.Contracts;
using IBApi.Errors;
using IBApi.MarketDepth;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class MarketDepthTests
    {
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();
            observerMock = new Mock<IMarketDepthObserver>();
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureWhatSubscriptionSendsRightUpdates()
        {
        }

        [TestMethod]
        public void EnsureWhatSubscriptionWorksWithErrors()
        {
            var marketDepthSubscription = CreateMarketDepthSubscription();

            var error = new ErrorMessage
            {
                ErrorCode = ErrorCode.DataFarmConnected,
                Message = "123",
                RequestId = ConnectionHelper.RequestId
            };

            connectionHelper.SendMessage(error);

            var expectedError = new Error
            {
                Code = ErrorCode.DataFarmConnected,
                Message = "123",
                RequestId = ConnectionHelper.RequestId
            };

            observerMock.Verify(observer => observer.OnError(expectedError), Times.Once);
            marketDepthSubscription.Dispose();
        }

        [TestMethod]
        public void EnsureWhatSubscriptionSendsUnsubscribeOnlyOnceOnDisposing()
        {
            var marketDepthSubscription = CreateMarketDepthSubscription();

            marketDepthSubscription.Dispose();
            marketDepthSubscription.Dispose();

            connectionHelper.EnsureWhatMessageSended(
                (RequestCancelMarketDepth message) => message.RequestId == ConnectionHelper.RequestId,
                Times.Once);
        }

        private MarketDepthSubscription CreateMarketDepthSubscription()
        {
            var contract = new Contract();
            var marketDepthSubscription = new MarketDepthSubscription(connectionHelper.Connection(), observerMock.Object, contract);
            return marketDepthSubscription;
        }

        private ConnectionHelper connectionHelper;
        private Mock<IMarketDepthObserver> observerMock;
    }
}
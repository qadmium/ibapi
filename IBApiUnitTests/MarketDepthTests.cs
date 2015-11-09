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
        private ConnectionHelper connectionHelper;
        private Mock<IMarketDepthObserver> observerMock;

        [TestInitialize]
        public void Init()
        {
            this.connectionHelper = new ConnectionHelper();
            this.observerMock = new Mock<IMarketDepthObserver>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureThatSubscriptionSendsRightUpdates()
        {
        }

        [TestMethod]
        public void EnsureThatSubscriptionWorksWithErrors()
        {
            var marketDepthSubscription = this.CreateMarketDepthSubscription();

            var error = new ErrorMessage
            {
                ErrorCode = ErrorCode.DataFarmConnected,
                Message = "123",
                RequestId = ConnectionHelper.RequestId
            };

            this.connectionHelper.SendMessage(error);

            var expectedError = new Error
            {
                Code = ErrorCode.DataFarmConnected,
                Message = "123",
                RequestId = ConnectionHelper.RequestId
            };

            this.observerMock.Verify(observer => observer.OnError(expectedError), Times.Once);
            marketDepthSubscription.Dispose();
        }

        [TestMethod]
        public void EnsureThatSubscriptionSendsUnsubscribeOnlyOnceOnDisposing()
        {
            var marketDepthSubscription = this.CreateMarketDepthSubscription();

            marketDepthSubscription.Dispose();
            marketDepthSubscription.Dispose();

            this.connectionHelper.EnsureThatMessageSended(
                (RequestCancelMarketDepth message) => message.RequestId == ConnectionHelper.RequestId,
                Times.Once);
        }

        private MarketDepthSubscription CreateMarketDepthSubscription()
        {
            var contract = new Contract();
            var marketDepthSubscription = new MarketDepthSubscription(this.connectionHelper.Connection(),
                this.observerMock.Object, contract);
            return marketDepthSubscription;
        }
    }
}
using IBApi;
using IBApi.Contracts;
using IBApi.Errors;
using IBApi.Messages.Client;
using IBApi.Messages.Server;
using IBApi.Quotes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class QuotesTests
    {
        private ConnectionHelper connectionHelper;
        private Mock<IQuoteObserver> observerMock;

        [TestInitialize]
        public void Init()
        {
            this.connectionHelper = new ConnectionHelper();
            this.observerMock = new Mock<IQuoteObserver>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureThatSubscriptionWorks()
        {
            var quoteSucription = this.CreateQuoteSubscription();

            {
                var message = new TickPriceMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Price = 1.05,
                    Size = 1,
                    TickType = TickType.Ask
                };

                this.connectionHelper.SendMessage(message);

                var expectedQuote = new Quote {AskPrice = 1.05, AskSize = 1};
                this.observerMock.Verify(observer => observer.OnQuote(expectedQuote), Times.Once);
            }

            {
                var message = new TickPriceMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Price = 2.05,
                    Size = 2,
                    TickType = TickType.Bid
                };

                this.connectionHelper.SendMessage(message);

                var expectedQuote = new Quote {AskPrice = 1.05, AskSize = 1, BidPrice = 2.05, BidSize = 2};
                this.observerMock.Verify(observer => observer.OnQuote(expectedQuote), Times.Once);
            }

            {
                var message = new TickPriceMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Price = 3.05,
                    Size = 3,
                    TickType = TickType.Last
                };

                this.connectionHelper.SendMessage(message);

                var expectedQuote = new Quote
                {
                    AskPrice = 1.05,
                    AskSize = 1,
                    BidPrice = 2.05,
                    BidSize = 2,
                    TradePrice = 3.05,
                    TradeSize = 3
                };

                this.observerMock.Verify(observer => observer.OnQuote(expectedQuote), Times.Once);
            }

            this.observerMock.Verify(observer => observer.OnError(It.IsAny<Error>()), Times.Never);
            quoteSucription.Dispose();
        }

        [TestMethod]
        public void EnsureThatSubscriptionWorksWithErrors()
        {
            var quoteSucription = this.CreateQuoteSubscription();

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
            quoteSucription.Dispose();
        }

        [TestMethod]
        public void EnsureThatSubscriptionSendsUnsubscribeOnlyOnceOnDisposing()
        {
            var quoteSucription = this.CreateQuoteSubscription();

            quoteSucription.Dispose();
            quoteSucription.Dispose();

            this.connectionHelper.EnsureThatMessageSended(
                (RequestCancelMarketData message) => message.RequestId == ConnectionHelper.RequestId,
                Times.Once);
        }

        private QuoteSubscription CreateQuoteSubscription()
        {
            var contract = new Contract();
            var quoteSucription = new QuoteSubscription(this.connectionHelper.Connection(), this.observerMock.Object,
                contract);
            return quoteSucription;
        }
    }
}
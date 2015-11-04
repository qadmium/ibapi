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
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();
            observerMock = new Mock<IQuoteObserver>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureWhatSubscriptionWorks()
        {
            var quoteSucription = CreateQuoteSubscription();

            {
                var message = new TickPriceMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Price = 1.05,
                    Size = 1,
                    TickType = TickType.Ask
                };

                connectionHelper.SendMessage(message);

                var expectedQuote = new Quote {AskPrice = 1.05, AskSize = 1};
                observerMock.Verify(observer => observer.OnQuote(expectedQuote), Times.Once);
            }

            {
                var message = new TickPriceMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Price = 2.05,
                    Size = 2,
                    TickType = TickType.Bid
                };

                connectionHelper.SendMessage(message);

                var expectedQuote = new Quote {AskPrice = 1.05, AskSize = 1, BidPrice = 2.05, BidSize = 2};
                observerMock.Verify(observer => observer.OnQuote(expectedQuote), Times.Once);
            }

            {
                var message = new TickPriceMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Price = 3.05,
                    Size = 3,
                    TickType = TickType.Last
                };

                connectionHelper.SendMessage(message);

                var expectedQuote = new Quote
                {
                    AskPrice = 1.05,
                    AskSize = 1,
                    BidPrice = 2.05,
                    BidSize = 2,
                    TradePrice = 3.05,
                    TradeSize = 3
                };

                observerMock.Verify(observer => observer.OnQuote(expectedQuote), Times.Once);
            }

            observerMock.Verify(observer => observer.OnError(It.IsAny<Error>()), Times.Never);
            quoteSucription.Dispose();
        }

        [TestMethod]
        public void EnsureWhatSubscriptionWorksWithErrors()
        {
            var quoteSucription = CreateQuoteSubscription();

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
            quoteSucription.Dispose();
        }

        [TestMethod]
        public void EnsureWhatSubscriptionSendsUnsubscribeOnlyOnceOnDisposing()
        {
            var quoteSucription = CreateQuoteSubscription();

            quoteSucription.Dispose();
            quoteSucription.Dispose();

            connectionHelper.EnsureWhatMessageSended(
                (RequestCancelMarketData message) => message.RequestId == ConnectionHelper.RequestId,
                Times.Once);
        }

        private QuoteSubscription CreateQuoteSubscription()
        {
            var contract = new Contract();
            var quoteSucription = new QuoteSubscription(connectionHelper.Connection(), observerMock.Object, contract);
            return quoteSucription;
        }

        private ConnectionHelper connectionHelper;
        private Mock<IQuoteObserver> observerMock;
    }
}
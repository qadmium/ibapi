using System;
using IBApi;
using IBApi.MarketDepth;
using IBApi.Messages.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class MarketDepthUpdatesDispatcherTests
    {
        [TestInitialize]
        public void Init()
        {
            observerMock = new Mock<IMarketDepthObserver>();
            dispatcher = new MarketDepthUpdatesDispatcher(observerMock.Object);
        }

        [TestMethod]
        public void EnsureWhatOnBidUpdateWillBeCalledOnInsertOnBidSide()
        {
            var update = new MarketDepthMessage
            {
                BidSide = true,
                Operation = MarketDepthOperation.Insert,
                Position = 1
            };

            dispatcher.OnMarketDepth(update);

            observerMock.Verify(observer => observer.OnBidUpdate(update.Position, It.IsAny<MarketDepthUpdate>()));
        }

        [TestMethod]
        public void EnsureWhatOnAskUpdateWillBeCalledOnInsertOnAskSide()
        {
            var update = new MarketDepthMessage
            {
                BidSide = false,
                Operation = MarketDepthOperation.Insert,
                Position = 1
            };

            dispatcher.OnMarketDepth(update);

            observerMock.Verify(observer => observer.OnAskUpdate(update.Position, It.IsAny<MarketDepthUpdate>()));
        }

        [TestMethod]
        public void EnsureWhatOnBidUpdateWillBeCalledOnUpdateOnBidSide()
        {
            var update = new MarketDepthMessage
            {
                BidSide = true,
                Operation = MarketDepthOperation.Update,
                Position = 1
            };

            dispatcher.OnMarketDepth(update);

            observerMock.Verify(observer => observer.OnBidUpdate(update.Position, It.IsAny<MarketDepthUpdate>()));
        }

        [TestMethod]
        public void EnsureWhatOnAskUpdateWillBeCalledOnUpdateOnAskSide()
        {
            var update = new MarketDepthMessage
            {
                BidSide = false,
                Operation = MarketDepthOperation.Insert,
                Position = 1
            };

            dispatcher.OnMarketDepth(update);

            observerMock.Verify(observer => observer.OnAskUpdate(update.Position, It.IsAny<MarketDepthUpdate>()));
        }

        [TestMethod]
        public void EnsureWhatOnBidRemoveWillBeCalledOnDeleteOnBidSide()
        {
            var update = new MarketDepthMessage
            {
                BidSide = true,
                Operation = MarketDepthOperation.Delete,
                Position = 1
            };

            dispatcher.OnMarketDepth(update);

            observerMock.Verify(observer => observer.OnBidRemove(update.Position));
        }

        [TestMethod]
        public void EnsureWhatOnAskRemoveWillBeCalledOnDeleteOnAskSide()
        {
            var update = new MarketDepthMessage
            {
                BidSide = false,
                Operation = MarketDepthOperation.Delete,
                Position = 1
            };

            dispatcher.OnMarketDepth(update);

            observerMock.Verify(observer => observer.OnAskRemove(update.Position));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void EnsureWhatExceptionWillBeTrownOnInvalidUpdateType()
        {
            var update = new MarketDepthMessage
            {
                BidSide = false,
                Operation = (MarketDepthOperation)12,
                Position = 1
            };

            dispatcher.OnMarketDepth(update);
        }

        private MarketDepthUpdatesDispatcher dispatcher;
        private Mock<IMarketDepthObserver> observerMock;
    }
}

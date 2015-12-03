using System;
using IBApi.Messages.Server;
using IBApi.Positions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

// ReSharper disable AccessToDisposedClosure

namespace IBApiUnitTests
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void EnsureThatPositionChangedRaisedOnUpdate()
        {
            var position = new Position();
            var callback = new Mock<EventHandler<PositionChangedEventArgs>>();
            position.PositionChanged += callback.Object;

            position.Update(new PortfolioValueMessage{SecurityType = "STK"}, "testaccount");

            callback.Verify(action => action(position, It.IsAny<PositionChangedEventArgs>()), Times.Once);
            position.Dispose();
        }
    }
}

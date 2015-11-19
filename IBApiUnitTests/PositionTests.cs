using IBApi.Messages.Server;
using IBApi.Positions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void EnsureThatPositionChangedRaisedOnUpdate()
        {
            var position = new Position();
            var callback = new Mock<PositionChangedEventHandler>();
            position.PositionChanged += callback.Object;

            position.Update(new PortfolioValueMessage{SecurityType = "STK"}, "testaccount");

            callback.Verify(action => action(position), Times.Once);
            position.Dispose();
        }
    }
}

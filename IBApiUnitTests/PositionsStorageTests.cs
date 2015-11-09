using IBApi;
using IBApi.Messages.Server;
using IBApi.Positions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class PositionsStorageTests
    {
        private ConnectionHelper connectionHelper;

        private Mock<IApiObjectsFactory> factoryMock;
        private PositionsStorage positionStorage;

        [TestInitialize]
        public void Init()
        {
            this.connectionHelper = new ConnectionHelper();
            this.factoryMock = new Mock<IApiObjectsFactory>();
            this.positionStorage = new PositionsStorage(this.connectionHelper.Connection(), this.factoryMock.Object,
                "testaccount");
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.connectionHelper.Dispose();
            this.positionStorage.Dispose();
        }

        [TestMethod]
        public void EnsureThatPositionsStorageRaisesEventOnlyOnNewPosition()
        {
            this.factoryMock.Setup(factory => factory.CreatePosition()).Returns(new Position());

            var onNewPositionCallback = new Mock<PositionAddedEventHandler>();
            this.positionStorage.PositionAdded += onNewPositionCallback.Object;

            this.connectionHelper.SendMessage(new PortfolioValueMessage
            {
                Symbol = "testsymbol",
                SecurityType = "STK",
                AccountName = "testaccount"
            });

            this.connectionHelper.SendMessage(new PortfolioValueMessage
            {
                Symbol = "testsymbol",
                SecurityType = "STK",
                AccountName = "testaccount"
            });

            onNewPositionCallback.Verify(callback => callback(It.IsAny<IPosition>()), Times.Once);

            Assert.AreEqual(1, this.positionStorage.Positions.Count);
        }
    }
}
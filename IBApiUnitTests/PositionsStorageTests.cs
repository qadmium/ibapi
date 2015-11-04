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
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();
            factoryMock = new Mock<IApiObjectsFactory>();
            positionStorage = new PositionsStorage(connectionHelper.Connection(), factoryMock.Object, "testaccount");
        }

        [TestCleanup]
        public void Cleanup()
        {
            connectionHelper.Dispose();
            positionStorage.Dispose();
        }

        [TestMethod]
        public void EnsureThatPositionsStorageInitializedAfterReceivingAccounts()
        {
            var initCallback = new Mock<InitializedEventHandler>();
            positionStorage.Initialized += initCallback.Object;

            Assert.IsFalse(positionStorage.IsInitialized);

            positionStorage.AccountsReceived();

            initCallback.Verify(callback => callback(), Times.Once);
            Assert.IsTrue(positionStorage.IsInitialized);
        }

        [TestMethod]
        public void EnsureThatPositionsStorageRaisesEventOnlyOnNewPosition()
        {
            factoryMock.Setup(factory => factory.CreatePosition()).Returns(new Position());

            var onNewPositionCallback = new Mock<PositionAddedEventHandler>();
            positionStorage.PositionAdded += onNewPositionCallback.Object;

            connectionHelper.SendMessage(new PortfolioValueMessage
            {
                Symbol = "testsymbol",
                SecurityType = "STK",
                AccountName = "testaccount",
            });

            connectionHelper.SendMessage(new PortfolioValueMessage
            {
                Symbol = "testsymbol",
                SecurityType = "STK",
                AccountName = "testaccount",
            });

            onNewPositionCallback.Verify(callback => callback(It.IsAny<IPosition>()), Times.Once);

            Assert.AreEqual(1, positionStorage.Positions.Count);
        }

        private Mock<IApiObjectsFactory> factoryMock;
        private ConnectionHelper connectionHelper;
        private PositionsStorage positionStorage;
    }
}

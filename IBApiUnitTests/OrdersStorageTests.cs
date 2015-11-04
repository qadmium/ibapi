using IBApi;
using IBApi.Messages.Server;
using IBApi.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class OrdersStorageTests
    {
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();
            factoryMock = new Mock<IApiObjectsFactory>();
            ordersStorage = new OrdersStorage(connectionHelper.Connection(), factoryMock.Object, "testaccount");
        }

        [TestCleanup]
        public void Cleanup()
        {
            connectionHelper.Dispose();
            ordersStorage.Dispose();
        }

        [TestMethod]
        public void EnsureThatOrdersStorageInitializedAfterReceivingAccounts()
        {
            var initCallback = new Mock<InitializedEventHandler>();
            ordersStorage.Initialized += initCallback.Object;

            Assert.IsFalse(ordersStorage.IsInitialized);

            ordersStorage.AccountsReceived();

            initCallback.Verify(callback => callback(), Times.Once);
            Assert.IsTrue(ordersStorage.IsInitialized);
        }

        [TestMethod]
        public void EnsureThatOrdersStorageRaisesEventOnlyOnNewPosition()
        {
            factoryMock.Setup(factory => factory.CreateOrder(It.IsAny<int>())).Returns(new Order(0, connectionHelper.Connection()));

            var onNewOrderCallback = new Mock<OrderAddedEventHandler>();
            ordersStorage.OrderAdded += onNewOrderCallback.Object;

            connectionHelper.SendMessage(new OpenOrderMessage
            {
                Symbol = "testsymbol",
                SecurityType = "STK",
                Account = "testaccount",
                OrderId = 0,
                OrderAction = "SELL",
                OrderType = "MKT"
            });

            connectionHelper.SendMessage(new OpenOrderMessage
            {
                Symbol = "testsymbol",
                SecurityType = "STK",
                Account = "testaccount",
                OrderId = 0,
                OrderAction = "SELL",
                OrderType = "MKT"
            });

            onNewOrderCallback.Verify(callback => callback(It.IsAny<IOrder>()), Times.Once);

            Assert.AreEqual(1, ordersStorage.Orders.Count);
        }

        private ConnectionHelper connectionHelper;
        private Mock<IApiObjectsFactory> factoryMock;
        private OrdersStorage ordersStorage;

    }
}

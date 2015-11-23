using IBApi;
using IBApi.Messages.Server;
using IBApi.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class OrderTests
    {
        private ConnectionHelper connectionHelper;
        private Order order;
        private Mock<IApiObjectsFactory> factoryMock;

        [TestInitialize]
        public void Init()
        {
            this.connectionHelper = new ConnectionHelper();
            this.factoryMock = new Mock<IApiObjectsFactory>();
            this.order = new Order(1, "testaccount", this.connectionHelper.Connection(), this.factoryMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.order.Dispose();
            this.connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureThatOrderRaisesChangeEventOnOpenOrderMessage()
        {
            var callbackMock = new Mock<OrderChangedEventHandler>();
            this.order.OrderChanged += callbackMock.Object;

            this.order.Update(new OpenOrderMessage
            {
                OrderId = 1,
                SecurityType = "STK",
                OrderAction = "SELL",
                OrderType = "MKT"
            });

            callbackMock.Verify(callback => callback(this.order), Times.Once);
        }

        [TestMethod]
        public void EnsureThatOrderRaisesChangeEventOnOrderStatusMessage()
        {
            var callbackMock = new Mock<OrderChangedEventHandler>();
            this.order.OrderChanged += callbackMock.Object;

            this.connectionHelper.SendMessage(new OrderStatusMessage
            {
                OrderId = 1
            });

            callbackMock.Verify(callback => callback(this.order), Times.Once);
        }
    }
}
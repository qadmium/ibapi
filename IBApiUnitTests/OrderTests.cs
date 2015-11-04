using IBApi.Messages.Server;
using IBApi.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class OrderTests
    {
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();
            order = new Order(1, connectionHelper.Connection());
        }

        [TestCleanup]
        public void Cleanup()
        {
            order.Dispose();
            connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureThatOrderRaisesChangeEventOnOpenOrderMessage()
        {
            var callbackMock = new Mock<OrderChangedEventHandler>();
            order.OrderChanged += callbackMock.Object;

            order.Update(new OpenOrderMessage
            {
                OrderId = 1,
                SecurityType = "STK",
                OrderAction = "SELL",
                OrderType = "MKT"
            });

            callbackMock.Verify(callback => callback(order), Times.Once);
        }

        [TestMethod]
        public void EnsureThatOrderRaisesChangeEventOnOrderStatusMessage()
        {
            var callbackMock = new Mock<OrderChangedEventHandler>();
            order.OrderChanged += callbackMock.Object;

            connectionHelper.SendMessage(new OrderStatusMessage
            {
                OrderId = 1
            });

            callbackMock.Verify(callback => callback(order), Times.Once);
        }

        private ConnectionHelper connectionHelper;
        private Order order;
    }
}

﻿using System;
using System.Threading;
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
        private ConnectionHelper connectionHelper;
        private Mock<IApiObjectsFactory> factoryMock;
        private OrdersStorage ordersStorage;

        [TestInitialize]
        public void Init()
        {
            this.connectionHelper = new ConnectionHelper();
            this.factoryMock = new Mock<IApiObjectsFactory>();
            this.ordersStorage = new OrdersStorage(this.connectionHelper.Connection(), this.factoryMock.Object,
                "testaccount");
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.connectionHelper.Dispose();
            this.ordersStorage.Dispose();
        }

        [TestMethod]
        public void EnsureThatOrdersStorageRaisesEventOnlyOnNewPosition()
        {
            this.factoryMock.Setup(factory => factory.CreateOrder(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new Order(0, "testaccount", this.connectionHelper.Connection(), this.factoryMock.Object,
                    new CancellationTokenSource()));

            var onNewOrderCallback = new Mock<EventHandler<OrderAddedEventArgs>>();
            this.ordersStorage.OrderAdded += onNewOrderCallback.Object;

            this.connectionHelper.SendMessage(new OpenOrderMessage
            {
                Symbol = "testsymbol",
                SecurityType = "STK",
                Account = "testaccount",
                OrderId = 0,
                OrderAction = "SELL",
                OrderType = "MKT"
            });

            this.connectionHelper.SendMessage(new OpenOrderMessage
            {
                Symbol = "testsymbol",
                SecurityType = "STK",
                Account = "testaccount",
                OrderId = 0,
                OrderAction = "SELL",
                OrderType = "MKT"
            });

            onNewOrderCallback.Verify(callback => callback(this.ordersStorage, It.IsAny<OrderAddedEventArgs>()), Times.Once);

            Assert.AreEqual(1, this.ordersStorage.Orders.Count);
        }
    }
}
using IBApi;
using IBApi.Accounts;
using IBApi.Executions;
using IBApi.Messages.Server;
using IBApi.Orders;
using IBApi.Positions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class AccountTests
    {
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();

            executionsStorageMock = new Mock<IExecutionStorageInternal>();
            positionsStorageMock = new Mock<IPositionsStorageInternal>();
            ordersStorageMock = new Mock<IOrdersStorageInternal>();

            factoryMock = new Mock<IApiObjectsFactory>();

            factoryMock.Setup(factory => factory.CreateExecutionStorage(It.IsAny<string>(), TODO))
                .Returns(executionsStorageMock.Object);

            factoryMock.Setup(factory => factory.CreatePositionStorage(It.IsAny<string>()))
                .Returns(positionsStorageMock.Object);

            factoryMock.Setup(factory => factory.CreateOrdersStorage(It.IsAny<string>()))
                .Returns(ordersStorageMock.Object);

            connectionHelper.Connection().Run();
            account = new Account("testaccount", connectionHelper.Connection(), factoryMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            account.Dispose();
            connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureThatBaseCurrencyMapsOnAccountFields()
        {
            Assert.AreEqual(0, account.Currencies.Length);

            connectionHelper.SendMessage(new AccountValueMessage
            {
                AccountName = "testaccount",
                Currency = "BASE",
                Key = "TotalCashBalance",
                Value = "100500"
            });

            Assert.AreEqual(100500, account.AccountFields.TotalCashBalance);

            connectionHelper.SendMessage(new AccountValueMessage
            {
                AccountName = "testaccount",
                Currency = "BASE",
                Key = "BuyingPower",
                Value = "500100"
            });

            Assert.AreEqual(500100, account.AccountFields.BuyingPower);
        }

        [TestMethod]
        public void EnsureThatNotBaseCurrencyMapsOnCurrencyAccountFields()
        {
            Assert.AreEqual(0, account.Currencies.Length);

            connectionHelper.SendMessage(new AccountValueMessage
            {
                AccountName = "testaccount",
                Currency = "USD",
                Key = "TotalCashBalance",
                Value = "100500"
            });

            Assert.AreEqual(100500, account["USD"].TotalCashBalance);
        }

        [TestMethod]
        public void EnsureThatAccountInitializedOnAllChildStoragesInitialized()
        {
            var initializationCallback = new Mock<InitializedEventHandler>();

            account.Initialized += initializationCallback.Object;

            Assert.IsFalse(account.IsInitialized);

            executionsStorageMock.Setup(executionsStorage => executionsStorage.IsInitialized).Returns(true);
            executionsStorageMock.Raise(executionsStorage => executionsStorage.Initialized += null);

            Assert.IsFalse(account.IsInitialized);

            positionsStorageMock.Setup(positionsStorage => positionsStorage.IsInitialized).Returns(true);
            positionsStorageMock.Raise(positionsStorage => positionsStorage.Initialized += null);

            Assert.IsFalse(account.IsInitialized);

            ordersStorageMock.Setup(ordersStorage => ordersStorage.IsInitialized).Returns(true);
            ordersStorageMock.Raise(ordersStorage => ordersStorage.Initialized += null);

            Assert.IsFalse(account.IsInitialized);

            var message = new AccountDownloadEndMessage
            {
                AccountName = "testaccount",
                Version = 0
            };

            connectionHelper.SendMessage(message);

            Assert.IsTrue(account.IsInitialized);
            initializationCallback.Verify(callback => callback(), Times.Once);
        }

        private ConnectionHelper connectionHelper;
        private Mock<IApiObjectsFactory> factoryMock;
        private Mock<IExecutionStorageInternal> executionsStorageMock;
        private Mock<IPositionsStorageInternal> positionsStorageMock;
        private Mock<IOrdersStorageInternal> ordersStorageMock;
        private Account account;
    }
}

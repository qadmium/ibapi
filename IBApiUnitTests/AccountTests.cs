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
        private Account account;

        private ConnectionHelper connectionHelper;
        private Mock<IExecutionStorageInternal> executionsStorageMock;
        private Mock<IOrdersStorageInternal> ordersStorageMock;
        private Mock<IPositionsStorageInternal> positionsStorageMock;
        private Mock<IOrdersIdsDispenser> ordersIdsDispenserMock;

        [TestInitialize]
        public void Init()
        {
            this.connectionHelper = new ConnectionHelper();

            this.executionsStorageMock = new Mock<IExecutionStorageInternal>();
            this.positionsStorageMock = new Mock<IPositionsStorageInternal>();
            this.ordersStorageMock = new Mock<IOrdersStorageInternal>();
            this.ordersIdsDispenserMock = new Mock<IOrdersIdsDispenser>();

            this.account = new Account("testaccount", this.connectionHelper.Connection(), this.executionsStorageMock.Object
                , this.positionsStorageMock.Object, this.ordersStorageMock.Object, this.ordersIdsDispenserMock.Object,
                new AccountCurrenciesFields());
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.account.Dispose();
            this.connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureThatBaseCurrencyMapsOnAccountFields()
        {
            Assert.AreEqual(0, this.account.Currencies.Length);

            this.connectionHelper.SendMessage(new AccountValueMessage
            {
                AccountName = "testaccount",
                Currency = "BASE",
                Key = "TotalCashBalance",
                Value = "100500"
            });

            Assert.AreEqual(100500, this.account.AccountFields.TotalCashBalance);

            this.connectionHelper.SendMessage(new AccountValueMessage
            {
                AccountName = "testaccount",
                Currency = "BASE",
                Key = "BuyingPower",
                Value = "500100"
            });

            Assert.AreEqual(500100, this.account.AccountFields.BuyingPower);
        }

        [TestMethod]
        public void EnsureThatNotBaseCurrencyMapsOnCurrencyAccountFields()
        {
            Assert.AreEqual(0, this.account.Currencies.Length);

            this.connectionHelper.SendMessage(new AccountValueMessage
            {
                AccountName = "testaccount",
                Currency = "USD",
                Key = "TotalCashBalance",
                Value = "100500"
            });

            Assert.AreEqual(100500, this.account["USD"].TotalCashBalance);
        }
    }
}
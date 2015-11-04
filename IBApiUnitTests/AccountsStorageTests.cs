using IBApi;
using IBApi.Accounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class AccountsStorageTests
    {
        [TestInitialize]
        public void Init()
        {
            connectionHelper = new ConnectionHelper();
            account1 = new Mock<IAccountInternal>();
            account2 = new Mock<IAccountInternal>();
            factoryMock = new Mock<IApiObjectsFactory>();

            factoryMock.Setup(factory => factory.CreateAccount(It.IsAny<string>()))
                .ReturnsInOrder(account1.Object, account2.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            connectionHelper.Dispose();
        }

        [TestMethod]
        public void EnsureWhatAccountsStorageInitializedWhenAllAccountsInitialized()
        {
            var accountsStorage = new AccountsStorage(new[] {"account1", "account2"}, connectionHelper.Connection(),
                factoryMock.Object);

            var initilizationCallback = new Mock<InitializedEventHandler>();
            accountsStorage.Initialized += initilizationCallback.Object;

            Assert.IsFalse(accountsStorage.IsInitialized);
            initilizationCallback.Verify(callback => callback(), Times.Never);

            account1.Setup(account => account.IsInitialized).Returns(true);
            account1.Raise(account => account.Initialized += null);

            Assert.IsFalse(accountsStorage.IsInitialized);
            initilizationCallback.Verify(callback => callback(), Times.Never);

            account2.Setup(account => account.IsInitialized).Returns(true);
            account2.Raise(account => account.Initialized += null);

            Assert.IsTrue(accountsStorage.IsInitialized);
            initilizationCallback.Verify(callback => callback(), Times.Once);

            accountsStorage.Dispose();
        }

        private Mock<IApiObjectsFactory> factoryMock;
        private Mock<IAccountInternal> account1;
        private Mock<IAccountInternal> account2;
        private ConnectionHelper connectionHelper;
    }
}

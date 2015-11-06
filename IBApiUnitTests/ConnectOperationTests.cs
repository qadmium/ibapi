using System;
using IBApi;
using IBApi.Accounts;
using IBApi.Connection;
using IBApi.Errors;
using IBApi.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IBApiUnitTests
{
    [TestClass]
    public class ConnectOperationTests
    {
        [TestInitialize]
        public void Init()
        {
            accountStorageMock = new Mock<IAccountsStorage>();
            connectionMock = new Mock<IConnection>();

            receiveManagedAccountsListOperationMock = new Mock<IReceiveManagedAccountsListOperation>();
            waitForMarketConnectedOperationMock = new Mock<IOperation>();

            factoryMock = new Mock<IApiObjectsFactory>();

            factoryMock.Setup(factory => factory.CreateReceiveManagedAccountsListOperation(TODO))
                .Returns(receiveManagedAccountsListOperationMock.Object);

            factoryMock.Setup(factory => factory.CreateWaitForMarketConnectedOperation(TODO))
                .Returns(waitForMarketConnectedOperationMock.Object);

            factoryMock.Setup(factory => factory.CreateAccountStorageOperation(It.IsAny<string[]>()))
                .Returns(accountStorageMock.Object);

            factoryMock.Setup(factory => factory.CreateClient(It.IsAny<IAccountsStorage>()))
                .Returns(new Mock<IClient>().Object);

            successCallback = new Mock<Action<IClient>>();
            failCallback = new Mock<Action<Error>>();

            receiveManagedAccountsListOperationMock.Setup(operation => operation.AccountsList)
                .Returns(new[] {"Acc1", "Acc2"});
        }

        [TestMethod]
        public void TestWhatOperationCompletedOnAllChildOperationsCompleted()
        {
            var connectOperation = CreateConnectOperation();

            connectOperation.Execute();

            receiveManagedAccountsListOperationMock.Raise(operation => operation.OperationCompleted += null);

            waitForMarketConnectedOperationMock.Setup(operation => operation.Completed).Returns(true);
            waitForMarketConnectedOperationMock.Raise(operation => operation.OperationCompleted += null);

            accountStorageMock.Setup(accountStorage => accountStorage.IsInitialized).Returns(true);
            accountStorageMock.Raise(accountStorage => accountStorage.Initialized += null);

            successCallback.Verify(callback => callback(It.IsAny<IClient>()), Times.Once);
            failCallback.Verify(callback => callback(It.IsAny<Error>()), Times.Never);

            connectOperation.Dispose();
        }

        [TestMethod]
        public void TestWhatOperationFailsOnConnectionError()
        {
            var connectOperation = CreateConnectOperation();

            connectOperation.Execute();

            receiveManagedAccountsListOperationMock.Raise(operation => operation.OperationCompleted += null);

            waitForMarketConnectedOperationMock.Raise(operation => operation.OperationFailed += null, new Error());

            accountStorageMock.Setup(accountStorage => accountStorage.IsInitialized).Returns(true);
            accountStorageMock.Raise(accountStorage => accountStorage.Initialized += null);

            successCallback.Verify(callback => callback(It.IsAny<IClient>()), Times.Never);
            failCallback.Verify(callback => callback(It.IsAny<Error>()), Times.Once);

            connectOperation.Dispose();
        }

        private ConnectOperation CreateConnectOperation()
        {
            var connectOperation = new ConnectOperation(factoryMock.Object, connectionMock.Object,
                successCallback.Object, failCallback.Object);
            return connectOperation;
        }

        private Mock<Action<IClient>> successCallback;
        private Mock<Action<Error>> failCallback;
        
        private Mock<IConnection> connectionMock;
        private Mock<IApiObjectsFactory> factoryMock;
        private Mock<IAccountsStorage> accountStorageMock;
        private Mock<IReceiveManagedAccountsListOperation> receiveManagedAccountsListOperationMock;
        private Mock<IOperation> waitForMarketConnectedOperationMock;
    }
}


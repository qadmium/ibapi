using System;
using IBApi.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class ErrorCodeExtensionsTests
    {
        [TestMethod]
        public void EnsureThatConnectionStatusRelatedWorks()
        {
            Assert.IsTrue(ErrorCode.MarketFarmConnected.ConnectionStatusRelated());
            Assert.IsTrue(ErrorCode.DataFarmConnected.ConnectionStatusRelated());
            Assert.IsTrue(ErrorCode.DataInactiveButAvailable.ConnectionStatusRelated());
        }

        [TestMethod]
        public void EnsureIsConnectedWorks()
        {
            Assert.IsTrue(ErrorCode.MarketFarmConnected.IsConnected());
            Assert.IsTrue(ErrorCode.DataFarmConnected.IsConnected());
            Assert.IsTrue(ErrorCode.DataInactiveButAvailable.IsConnected());
        }

        [TestMethod]
        public void EnsureIsGeneralErrorWorks()
        {
            Assert.IsFalse(ErrorCode.MarketFarmConnected.IsGeneralError());
            Assert.IsFalse(ErrorCode.DataFarmConnected.IsGeneralError());
            Assert.IsFalse(ErrorCode.DataInactiveButAvailable.IsGeneralError());
        }
    }
}

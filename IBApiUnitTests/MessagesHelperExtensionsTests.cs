using System;
using IBApi.Messages.Client.MessagesHelperExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class MessagesHelperExtensionsTests
    {
        [TestMethod]
        public void EnsureWhatToRightStringReturnsEmptyStringOnNull()
        {
            var result = ((bool?) null).ToRightString();

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void EnsureWhatToRightStringReturnsCOnCall()
        {
            var result = ((bool?)true).ToRightString();

            Assert.AreEqual("C", result);
        }

        [TestMethod]
        public void EnsureWhatToRightStringReturnsPOnCall()
        {
            var result = ((bool?)false).ToRightString();

            Assert.AreEqual("P", result);
        }
    }
}

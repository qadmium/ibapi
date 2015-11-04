using System;
using IBApi.Messages.Client.MessagesHelperExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class MessagesHelperExtensionsTests
    {
        [TestMethod]
        public void EnsureThatToRightStringReturnsEmptyStringOnNull()
        {
            var result = ((bool?) null).ToRightString();

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void EnsureThatToRightStringReturnsCOnCall()
        {
            var result = ((bool?)true).ToRightString();

            Assert.AreEqual("C", result);
        }

        [TestMethod]
        public void EnsureThatToRightStringReturnsPOnCall()
        {
            var result = ((bool?)false).ToRightString();

            Assert.AreEqual("P", result);
        }
    }
}

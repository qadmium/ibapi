using System;
using IBApi.Contracts;
using IBApi.Messages.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBApiUnitTests
{
    [TestClass]
    public class ContractTests
    {
        [TestMethod]
        public void EnsurePutCallStringParsing()
        {
            {
                var message = new ContractDataMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Symbol = "MSFT",
                    SecurityType = "OPT",
                    Right = "P"
                };

                var contract = Contract.FromContractDataMessage(message);

                Assert.AreEqual(false, contract.Call);    
            }

            {
                var message = new ContractDataMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Symbol = "MSFT",
                    SecurityType = "OPT",
                    Right = "C"
                };

                var contract = Contract.FromContractDataMessage(message);

                Assert.AreEqual(true, contract.Call);
            }

            {
                var message = new ContractDataMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Symbol = "MSFT",
                    SecurityType = "OPT",
                };

                var contract = Contract.FromContractDataMessage(message);

                Assert.AreEqual(null, contract.Call);
            }
        }

        [TestMethod]
        public void EnsureStrikeParsing()
        {
            {
                var message = new ContractDataMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Symbol = "MSFT",
                    SecurityType = "OPT",
                    Strike = 0.0
                };

                var contract = Contract.FromContractDataMessage(message);

                Assert.AreEqual(null, contract.Strike);
            }

            {
                var message = new ContractDataMessage
                {
                    RequestId = ConnectionHelper.RequestId,
                    Symbol = "MSFT",
                    SecurityType = "OPT",
                    Strike = 0.01
                };

                var contract = Contract.FromContractDataMessage(message);

                Assert.AreEqual(message.Strike, contract.Strike);
            }
        }

        [TestMethod]
        public void EnsureExpirationParsing()
        {
            var message = new ContractDataMessage
            {
                RequestId = ConnectionHelper.RequestId,
                Symbol = "MSFT",
                SecurityType = "OPT",
                
            };

            {
                message.ContractMonth = "201401";
                var contract = Contract.FromContractDataMessage(message);
                Assert.AreEqual(new DateTime(2014, 1, 1), contract.AdditionalContractInfo.ContractMonth);
            }

            {
                message.ContractMonth = "";
                var contract = Contract.FromContractDataMessage(message);
                Assert.AreEqual(null, contract.AdditionalContractInfo.ContractMonth);
            }

            {
                message.ContractMonth = "201436";
                var contract = Contract.FromContractDataMessage(message);
                Assert.AreEqual(null, contract.AdditionalContractInfo.ContractMonth);
            }

            {
                message.ContractMonth = "20140A";
                var contract = Contract.FromContractDataMessage(message);
                Assert.AreEqual(null, contract.AdditionalContractInfo.ContractMonth);
            }
        }
    }
}

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests
{
    [TestClass]
    public class DomainTests : TestBase
    {
        [TestMethod]
        public void GetInfo_ReturnsInformationOnExistingDomain()
        {
            DomainInfoResult info = _api.Domains.GetInfo(_domainName);
            Assert.IsTrue(info.ID > 0);
        }

        [TestMethod]
        public void GetList_ShouldContainTheTestDomain()
        {
            DomainListResult result = _api.Domains.GetList();
            Assert.IsTrue(result.Domains.Length > 0);
            Assert.IsTrue(result.Domains.Any(d => string.Equals(d.Name, _domainName)));
        }

        [TestMethod, Ignore("Needs work - can only renew a domain so many times")]
        public void Test_renew()
        {
            var result = _api.Domains.Renew(_domainName, 1);

            Assert.AreEqual(result.DomainName, _domainName);
            Assert.IsTrue(result.DomainID > 0);
            Assert.AreEqual(result.Renew, true);
            Assert.IsTrue(result.OrderID > 0);
            Assert.IsTrue(result.TransactionID > 0);
            Assert.IsTrue(result.ChargedAmount > 0);
        }

        [TestMethod, Ignore("Needs work - can only reactivate an expired domain")]
        public void Test_reactivate()
        {
            var result = _api.Domains.Reactivate(_domainName);

            Assert.AreEqual(result.DomainName, _domainName);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsTrue(result.OrderID > 0);
            Assert.IsTrue(result.TransactionID > 0);
            Assert.IsTrue(result.ChargedAmount > 0);
        }
    }
}

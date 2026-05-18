using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests.Domain
{
    [TestClass]
    public class DomainTests : TestBase
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            // hack to avoid Too many requests errors.
            // TODO: remove with https://github.com/SteveHanna/Namecheap-dot-net/issues/10
            System.Threading.Thread.Sleep(TestThrottleMilliseconds);
        }
        
        [TestMethod]
        public void GetInfo_ReturnsInformationOnExistingDomain()
        {
            DomainInfoResult info = _api.Domains.GetInfo(_domainName.Value);
            Assert.IsTrue(info.ID > 0);
        }

        [TestMethod]
        public void GetList_ShouldContainTheTestDomain()
        {
            DomainListResult result = _api.Domains.GetList();
            Assert.IsTrue(result.Domains.Length > 0);
            Assert.IsTrue(result.Domains.Any(d => string.Equals(d.Name, _domainName.Value)));
        }

        [TestMethod, Ignore("Will need to figure out a way to test with more than 20 domains")]
        public void GetList_ShouldContainMoreThan20Domains()
        {
            DomainListResult result = _api.Domains.GetList(1, 21);
            Assert.IsTrue(result.Domains.Length > 20);
        }

        [TestMethod]
        public void GetList_TestForAllParamsMethodShouldContainTheTestDomain()
        {
            string searchTerm = _domainName.Value.Substring(0, (_domainName.Value.Length - 4)); // for the .com TLD
            DomainListResult result = _api.Domains.GetList("ALL", searchTerm, 1, 21, "NAME");
            Assert.IsTrue(result.Domains.Length > 0);
            Assert.IsTrue(result.Domains.Any(d => string.Equals(d.Name, _domainName.Value)));
        }


        [TestMethod, Ignore("Needs work - can only renew a domain so many times")]
        public void Test_renew()
        {
            var result = _api.Domains.Renew(_domainName.Value, 1);

            Assert.AreEqual(result.DomainName, _domainName.Value);
            Assert.IsTrue(result.DomainID > 0);
            Assert.AreEqual(result.Renew, true);
            Assert.IsTrue(result.OrderID > 0);
            Assert.IsTrue(result.TransactionID > 0);
            Assert.IsTrue(result.ChargedAmount > 0);
        }

        [TestMethod, Ignore("Needs work - can only reactivate an expired domain")]
        public void Test_reactivate()
        {
            var result = _api.Domains.Reactivate(_domainName.Value);

            Assert.AreEqual(result.DomainName, _domainName.Value);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsTrue(result.OrderID > 0);
            Assert.IsTrue(result.TransactionID > 0);
            Assert.IsTrue(result.ChargedAmount > 0);
        }
    }
}

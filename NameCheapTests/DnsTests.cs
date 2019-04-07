using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests
{
    [TestClass]
    public class DnsTests : TestBase
    {
        [TestMethod, Ignore("Needs work - Should ensure the test is isolated or that the asserts are pre-set")]
        public void Test_getList()
        {
            var z = _api.Dns.GetList(_domainName.Replace(".com", ""), "com");
            Assert.IsTrue(z.IsUsingOurDns);
        }

        [TestMethod, Ignore("Needs work - Should assert that the DNS has been set")]
        public void Test_setCustom()
        {
            _api.Dns.SetCustom(_domainName.Replace(".com", ""), "com", "dns1.name-services.com", "dns2.name-services.com");
        }

        [TestMethod, Ignore("Needs work - Should assert that the default DNS has been set")]
        public void Test_setDefault()
        {
            _api.Dns.SetDefault(_domainName.Replace(".com", ""), "com");
        }
    }
}

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests.Dns
{
    [TestClass]
    public class DnsTests : TestBase
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            var (secondLevel, tld) = DomainParts.Value;

            // sets the DNS back to default
            _api.Dns.SetDefault(secondLevel, tld);
        }

        [TestMethod]
        public void SetCustom_SetsMultipleNameServers_WhenMultipleArePassed()
        {
            var (secondLevel, tld) = DomainParts.Value;
            string dns1 = "dns1.name-services.com", dns2 = "dns2.name-services.com";

            // act
            _api.Dns.SetCustom(secondLevel, tld, dns1, dns2);

            DnsListResult hostResult = _api.Dns.GetList(secondLevel, tld);
            Assert.IsFalse(hostResult.IsUsingOurDns);

            Assert.AreEqual(2, hostResult.NameServers.Count);
            Assert.IsTrue(hostResult.NameServers.Any(s => string.Equals(s, dns1, System.StringComparison.OrdinalIgnoreCase)));
            Assert.IsTrue(hostResult.NameServers.Any(s => string.Equals(s, dns2, System.StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void SetDefault_ReturnsTheDomainBackToDefault()
        {
            var (secondLevel, tld) = DomainParts.Value;

            // Arrange - forces the domain to a custom domain
            SetCustom_SetsMultipleNameServers_WhenMultipleArePassed();

            // sets it back to default
            _api.Dns.SetDefault(secondLevel, tld);

            DnsListResult hostResult = _api.Dns.GetList(secondLevel, tld);
            Assert.IsTrue(hostResult.IsUsingOurDns);
        }
    }
}

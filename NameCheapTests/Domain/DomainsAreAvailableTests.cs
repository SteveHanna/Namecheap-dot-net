using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests.Domain
{
    [TestClass]
    public class DomainsAreAvailableTests : TestBase
    {
        private readonly string _uniqueSite = $"{Guid.NewGuid()}.com";
        private static readonly string[] KnownDomains = { "google.com", "bing.com" };
        
        [TestMethod]
        public void AreAvailable_HandlesAvailableDomains()
        {
            DomainCheckResult[] domainNames = _api.Domains.AreAvailable(_uniqueSite);
            Assert.AreEqual(1, domainNames.Length);
            Assert.IsTrue(domainNames[0].IsAvailable);
            Assert.AreEqual(_uniqueSite, domainNames[0].DomainName, ignoreCase: true);
        }

        [TestMethod]
        public void AreAvailable_HandlesUnAvailableDomains()
        {
            DomainCheckResult[] domainNames = _api.Domains.AreAvailable(KnownDomains[0]);
            Assert.AreEqual(1, domainNames.Length);
            Assert.IsFalse(domainNames[0].IsAvailable);
            Assert.AreEqual(KnownDomains[0], domainNames[0].DomainName, ignoreCase: true);
        }

        [TestMethod]
        public void AreAvailable_ReturnsMultipleDomains_WhenMultipleParametersArePassed()
        {
            var domainNames = _api.Domains.AreAvailable(KnownDomains[0], KnownDomains[1]);
            Assert.IsFalse(domainNames.Single(o => string.Equals(o.DomainName, KnownDomains[0], StringComparison.OrdinalIgnoreCase)).IsAvailable);
            Assert.IsFalse(domainNames.Single(o => string.Equals(o.DomainName, KnownDomains[1], StringComparison.OrdinalIgnoreCase)).IsAvailable);
        }

        [TestMethod, Ignore("Needs to be written")]
        public void AreAvailable_OnlyReturnsInfoForParametersThatAreNotEmpty()
        {
            var domainNames = _api.Domains.AreAvailable(KnownDomains[0], "", _uniqueSite, null);
            Assert.AreEqual(2, domainNames.Length);
            Assert.IsFalse(domainNames.Single(o => o.DomainName == KnownDomains[0]).IsAvailable);
            Assert.IsTrue(domainNames.Single(o => o.DomainName == _uniqueSite).IsAvailable);
        }

        [TestMethod, Ignore("Needs to be written")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AreAvailable_ThrowsArgumentException_WhenAllArgumentsAreEmptyNullOrWhitespace()
        {
            _ = _api.Domains.AreAvailable("", null, "  ", "\t");
        }
    }
}

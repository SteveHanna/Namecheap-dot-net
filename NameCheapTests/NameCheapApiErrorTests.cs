using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests
{
    [TestClass]
    public class NameCheapApiErrorTests : TestBase
    {
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void DomainAreAvailable_ShouldThrow_WhenMisconfigured()
        {
            var api = new NameCheapApi("x", "x", "x", "x", true);
            _ = api.Domains.AreAvailable("google.com", _domainName);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Test_errors()
        {
            var api = new NameCheapApi("x", "x", "x", "x", true);
            _ = api.Dns.GetList(_domainName.Replace(".com", ""), "com");
        }
    }
}

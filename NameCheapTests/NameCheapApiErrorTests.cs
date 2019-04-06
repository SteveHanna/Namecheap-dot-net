using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests
{
    [TestClass]
    public class NameCheapApiErrorTests : TestBase
    {
        [TestMethod]
        public void Constructor_Throws_WhenUserNameIsMissing()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi(null, "test", "test", "0.0.0.0", isSandbox: true),
                "Expected constructor to throw an argument exception when username is null.");
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("", "test", "test", "0.0.0.0", isSandbox: true),
                "Expected constructor to throw an argument exception when username is empty.");
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("  ", "test", "test", "0.0.0.0", isSandbox: true),
                "Expected constructor to throw an argument exception when username is whitespace.");
        }

        [TestMethod]
        public void Constructor_Throws_WhenApiUserIsMissing()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("test", null,  "test", "0.0.0.0", isSandbox: true),
                "Expected constructor to throw an argument exception when apiUser is null.");
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("test", "", "test", "0.0.0.0", isSandbox: true),
                "Expected constructor to throw an argument exception when apiUser is empty.");
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("test", "  ", "test", "0.0.0.0", isSandbox: true),
                "Expected constructor to throw an argument exception when apiUser is whitespace.");
        }

        [TestMethod]
        public void Constructor_Throws_WhenApiKeyIsMissing()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("test", "test", null, "0.0.0.0", isSandbox: true),
                "Expected constructor to throw an argument exception when apiKey is null.");
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("test", "test", "", "0.0.0.0", isSandbox: true),
                "Expected constructor to throw an argument exception when apiKey is empty.");
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("test", "test", "  ", "0.0.0.0", isSandbox: true),
                "Expected constructor to throw an argument exception when apiKey is whitespace.");
        }

        [TestMethod]
        public void Constructor_Throws_WhenClientIpIsMissing()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("test", "test", "test", null, isSandbox: true),
                "Expected constructor to throw an argument exception when clientIp is null.");
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("test", "test", "test", "", isSandbox: true),
                "Expected constructor to throw an argument exception when clientIp is empty.");
            Assert.ThrowsException<ArgumentNullException>(
                () => new NameCheapApi("test", "test", "test", "  ", isSandbox: true),
                "Expected constructor to throw an argument exception when clientIp is whitespace.");
        }

        [TestMethod]
        public void Constructor_Throws_WhenClientIpIsNotAValidIp()
        {
            Assert.ThrowsException<ArgumentException>(
                () => new NameCheapApi("test", "test", "test", "test", isSandbox: true),
                "Expected constructor to throw an argument exception when clientIp is not a valid IP address.");
        }

        [TestMethod]
        public void Constructor_Throws_WhenClientIpIsNotAValidIpV4()
        {
            Assert.ThrowsException<ArgumentException>(
                () => new NameCheapApi("test", "test", "test", "::1", isSandbox: true),
                "Expected constructor to throw an argument exception when clientIp is not a valid IPv4.");
            Assert.ThrowsException<ArgumentException>(
                () => new NameCheapApi("test", "test", "test", "256.0.0.1", isSandbox: true),
                "Expected constructor to throw an argument exception when clientIp is not a valid IPv4.");
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void DomainAreAvailable_ShouldThrow_WhenMisconfigured()
        {
            var api = new NameCheapApi("x", "x", "x", "0.0.0.0", true);
            _ = api.Domains.AreAvailable("google.com", _domainName);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Test_errors()
        {
            var api = new NameCheapApi("x", "x", "x", "0.0.0.0", true);
            _ = api.Dns.GetList(_domainName.Replace(".com", ""), "com");
        }
    }
}

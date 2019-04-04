using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests
{
    [TestClass]
    public class DnsTests : TestBase
    {
        [TestMethod, Ignore("Needs work - should set the initial value to a default and then verify that the host entry was set properly")]
        public void Test_setdHost()
        {
            _api.Dns.SetHosts(new DnsHostsRequest()
            {
                SLD = _domainName.Replace(".com", ""),
                TLD = "com",
                HostEntries = new HostEntry[] { new HostEntry() { 
                            Address = "184.72.232.222",
                            HostName = "@",
                            RecordType =  RecordType.A
                } }
            });
        }

        [TestMethod, Ignore("Needs work - Should ensure this mailbox does not exist before setting and assert it does after")]
        public void Test_SetEmailForwarding()
        {
            EmailForwarding[] request = { new EmailForwarding() { MailBox = "example1", ForwardTo = "example@example.com" } };
            _api.Dns.SetEmailForwarding(_domainName, request);
        }

        [TestMethod, Ignore("Needs work - Should ensure the test is isolate and not rely on SetEmailForward to have been run")]
        public void Test_GetEmailForwarding()
        {
            var x = _api.Dns.GetEmailForwarding(_domainName);
            Assert.AreEqual("example@example.com", x.Emails.Single(o => o.MailBox == "example1").ForwardTo);
        }

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

        [TestMethod, Ignore("Needs work - Should ensure the test is isolate and the hosts have been set in the arrange phase")]
        public void Test_GetHosts()
        {
            var hosts = _api.Dns.GetHosts(_domainName.Replace(".com", ""), "com");
            Assert.IsTrue(hosts.IsUsingOurDns);
            Assert.AreEqual(hosts.HostEntries[0].RecordType, RecordType.A);
            Assert.AreEqual(hosts.HostEntries[0].HostName, "@");
        }

    }
}

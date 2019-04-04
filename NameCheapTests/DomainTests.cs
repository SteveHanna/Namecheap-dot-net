using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests
{
    [TestClass]
    public class DomainTests : TestBase
    {
        private NameCheapApi _api = new NameCheapApi(_apiUser.Value, _apiUser.Value, _apiKey.Value, _clientIp.Value, isSandbox: true);
        private string _domainName = "eaba62ff-e035-417a-8760-bd2d33972a25.com";

        [TestMethod]
        public void Test_create()
        {
            string domainName = Guid.NewGuid() + ".com";

            var contact = new ContactInformation()
            {
                Address1 = "1 never never land",
                City = "New York",
                Country = "US",
                EmailAddress = "noreply@example.com",
                FirstName = "Billy",
                LastName = "Bob",
                Phone = "+011.5555555555",
                PostalCode = "l5Z5Z5",
                StateProvince = "California"
            };

            var domain = _api.Domains.Create(new DomainCreateRequest()
            {
                DomainName = domainName,
                Admin = contact,
                AuxBilling = contact,
                Registrant = contact,
                Tech = contact,
                Years = 1
            });

            Assert.AreEqual(domainName, domain.Domain);
        }

        [TestMethod]
        public void Test_domain_check()
        {
            var uniqueSite = Guid.NewGuid() + ".com";

            var domainNames = _api.Domains.AreAvailable("google.com", uniqueSite);
            Assert.IsFalse(domainNames.Single(o => o.DomainName == "google.com").IsAvailable);
            Assert.IsTrue(domainNames.Single(o => o.DomainName == uniqueSite).IsAvailable);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Test_errors()
        {
            NameCheapApi api = new NameCheapApi("x", "x", "x", "x", true);
            var domainNames = api.Domains.AreAvailable("google.com", _domainName);
        }

        [TestMethod]
        public void Test_getcontacts()
        {
            var contacts = _api.Domains.GetContacts(_domainName);

            Assert.AreEqual(contacts.Admin.FirstName, "Billy");
            Assert.AreEqual(contacts.Admin.LastName, "Bob");
        }

        [TestMethod]
        public void Test_getInfo()
        {
            var info = _api.Domains.GetInfo(_domainName);
            Assert.AreEqual(14950, info.ID);
        }

        [TestMethod]
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

        //[TestMethod]
        public void Test_reactivate()
        {
            var result = _api.Domains.Reactivate(_domainName);

            Assert.AreEqual(result.DomainName, _domainName);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.IsTrue(result.OrderID > 0);
            Assert.IsTrue(result.TransactionID > 0);
            Assert.IsTrue(result.ChargedAmount > 0);
        }

        [TestMethod]
        public void Test_get_TLD_list()
        {
            var result = _api.Domains.GetTldList();
            Assert.AreEqual("Canada Country TLD", result.Tlds.Single(o => o.Name == "ca").LongName);
        }

        [TestMethod]
        public void Test_get_list()
        {
            var result = _api.Domains.GetList();
            Assert.IsTrue(result.Domains.Length > 0);
        }

        [TestMethod]
        public void Test_set_registrar_lock()
        {
            _api.Domains.SetRegistrarLock(_domainName);
        }

        [TestMethod]
        public void Test_get_registrar_lock()
        {
            bool isLocked = _api.Domains.GetRegistrarLock(_domainName);
            Assert.IsTrue(isLocked);
        }

        [TestMethod]
        public void Test_set_Contacts()
        {
            var contact = new ContactInformation()
            {
                Address1 = "1 never never land",
                City = "New York",
                Country = "US",
                EmailAddress = "noreply@example.com",
                FirstName = "Billy",
                LastName = "Bob",
                Phone = "+011.5555555555",
                PostalCode = "l5Z5Z5",
                StateProvince = "California"
            };

            _api.Domains.SetContacts(new DomainContactsRequest()
            {
                DomainName = _domainName,
                Admin = contact,
                AuxBilling = contact,
                Registrant = contact,
                Tech = contact
            });
        }
    }
}

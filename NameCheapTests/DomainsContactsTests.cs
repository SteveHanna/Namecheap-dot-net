using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests
{
    [TestClass]
    public class DomainsContactsTests : TestBase
    {
        [TestMethod]
        public void SetContacts_SetsAllContacts()
        {
            var timeNonce = DateTime.Now.Ticks;

            ContactInformation MakeContact(string type)
            {
                // keep type short - timeNonce is 18-chars long and good amount of fields are 50-chars long
                var nonce = $"{type}{timeNonce}";
                return new ContactInformation
                    {
                        Address1 = $"1 never never land {nonce}",
                        City = $"New York {nonce}",
                        Country = "US", // cannot add nonce to country as it validates against known list
                        EmailAddress = $"noreply_{nonce}@example.com",
                        FirstName = $"{TestUserFirstName}-{nonce}",
                        LastName = $"{TestUserLastName}-{nonce}",
                        Phone = "+011.5555555555", // cannot add same nonce as the number is required to be in a certain format
                        PostalCode = $"123-{nonce}",
                        StateProvince = $"California {nonce}"
                    };
            };

            var adminContact = MakeContact("Adm");
            var auxContact = MakeContact("AB");
            var techContact = MakeContact("T");
            var regContact = MakeContact("R");

            // Act
            _api.Domains.SetContacts(new DomainContactsRequest()
            {
                DomainName = _domainName,
                Admin = adminContact,
                AuxBilling = auxContact,
                Registrant = regContact,
                Tech = techContact
            });

            // Assert
            DomainContactsResult contacts = _api.Domains.GetContacts(_domainName);

            Assert.IsTrue(contacts.Admin.Equals(adminContact), "Admin contacts should match.");
            Assert.IsTrue(contacts.AuxBilling.Equals(auxContact), "Aux billing contacts should match.");
            Assert.IsTrue(contacts.Registrant.Equals(regContact), "Registrant contacts should match.");
            Assert.IsTrue(contacts.Tech.Equals(techContact), "Tech contacts should match.");
        }

        [TestMethod, Ignore("To be written")]
        public void SetContacts_Errors_WhenInvalidEmailAddress()
        {
            Assert.Fail("Test needs to be written.");
        }

        [TestMethod, Ignore("To be written")]
        public void SetContacts_Errors_WhenInvalidPhoneNumber()
        {
            Assert.Fail("Test needs to be written.");
        }

        [TestMethod, Ignore("To be written")]
        public void SetContacts_Truncates_WhenTextLongerThanLimits()
        {
            Assert.Fail("Test needs to be written.");
        }

        [TestMethod, Ignore("To be written")]
        public void SetContacts_Errors_WhenMissingAdminContacts()
        {
            Assert.Fail("Test needs to be written.");
        }

        [TestMethod, Ignore("To be written")]
        public void SetContacts_Errors_WhenMissingAuxBillingContacts()
        {
            Assert.Fail("Test needs to be written.");
        }

        [TestMethod, Ignore("To be written")]
        public void SetContacts_Errors_WhenMissingRegistrantContacts()
        {
            Assert.Fail("Test needs to be written.");
        }

        [TestMethod, Ignore("To be written")]
        public void SetContacts_Errors_WhenMissingTechContacts()
        {
            Assert.Fail("Test needs to be written.");
        }

        [TestMethod, Ignore("To be written")]
        public void SetContacts_Errors_WhenMissingExtendedAttributesForSomeDomains()
        {
            // Extended attributes - Required for .us, .eu, .ca, .co.uk, .org.uk, .me.uk, .nu , .asia, .com.au, .net.au, .org.au, .es, .nom.es, .com.es, .org.es, .de, .fr TLDs only
            Assert.Fail("Test needs to be written.");
        }
    }
}

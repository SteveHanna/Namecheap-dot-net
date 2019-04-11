using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NameCheap
{
    /// <summary>Set of functions responsible for domain management: creating, info, renewal, contacts, etc.</summary>
    public class DomainsApi
    {
        private readonly XNamespace _ns = XNamespace.Get("http://api.namecheap.com/xml.response");
        private readonly GlobalParameters _params;

        internal DomainsApi(GlobalParameters globalParams)
        {
            _params = globalParams;
        }

        /// <summary>
        ///  Checks the availability of domains.
        /// </summary>
        /// <param name="domains">Domains to check.</param>
        /// <returns>List of results for each parameter. Order is not guaranteed to match the order of parameters.</returns>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 3031510	Error response from Enom when the error count != 0
        /// - 3011511	Unknown response from the provider
        /// - 2011169	Only 50 domains are allowed in a single check command
        /// </exception>
        public DomainCheckResult[] AreAvailable(params string[] domains)
        {
            XDocument doc = new Query(_params)
                .AddParameter("DomainList", string.Join(",", domains))
                .Execute("namecheap.domains.check");

            return doc.Root.Element(_ns + "CommandResponse").Elements()
                   .Select(o => new DomainCheckResult()
                   {
                       DomainName = o.Attribute("Domain").Value,
                       IsAvailable = o.Attribute("Available").Value.Equals("true", StringComparison.OrdinalIgnoreCase)
                   }).ToArray();
        }

        /// <summary>
        /// Registers a new domain.
        /// </summary>
        /// <param name="domain">Information about domain to register.</param>
        /// <returns>Information about the created domain.</returns>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2033409	Possibly a logical error at the authentication phase. The order chargeable for the Username is not found
        /// - 2033407, 2033270	Cannot enable Whoisguard when AddWhoisguard is set to NO
        /// - 2015182	Contact phone is invalid. The phone number format is +NNN.NNNNNNNNNN
        /// - 2015267	EUAgreeDelete option should not be set to NO
        /// - 2011170	Validation error from PromotionCode
        /// - 2011280	Validation error from TLD
        /// - 2015167	Validation error from Years
        /// - 2030280	TLD is not supported in API
        /// - 2011168	Nameservers are not valid
        /// - 2011322	Extended Attributes are not valid
        /// - 2010323	Check the required field for billing domain contacts
        /// - 2528166	Order creation failed
        /// - 3019166, 4019166	Domain not available
        /// - 3031166	Error while getting information from the provider
        /// - 3028166	Error from Enom ( Errcount <> 0 )
        /// - 3031900	Unknown response from the provider
        /// - 4023271	Error while adding a free PositiveSSL for the domain
        /// - 3031166	Error while getting a domain status from Enom
        /// - 4023166	Error while adding a domain
        /// - 5050900	Unknown error while adding a domain to your account
        /// - 4026312	Error in refunding funds
        /// - 5026900	Unknown exceptions error while refunding funds
        /// - 2515610	Prices do not match
        /// - 2515623	Domain is premium while considered regular or is regular while considered premium
        /// - 2005	Country name is not valid
        /// </exception>
        public DomainCreateResult Create(DomainCreateRequest domain)
        {
            var query = new Query(_params);

            foreach (var item in GetNamesAndValuesFromProperties(domain))
                query.AddParameter(item.Key, item.Value);

            XDocument doc = query.Execute("namecheap.domains.create");
            XElement result = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainCreateResult");

            var serializer = new XmlSerializer(typeof(DomainCreateResult), _ns.NamespaceName);
            using (var reader = result.CreateReader())
            {
                return (DomainCreateResult)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Gets contact information for the requested domain.
        /// </summary>
        /// <param name="domain">Domain to get contacts.</param>
        /// <returns>All the contacts, Admin, AuxBilling, Registrant, and Tech for the domain.</returns>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found
        /// - 2016166	Domain is not associated with your account
        /// - 4019337	Unable to retrieve domain contacts
        /// - 3016166	Domain is not associated with Enom
        /// - 3019510	This domain has expired/ was transferred out/ is not associated with your account
        /// - 3050900	Unknown response from provider
        /// - 5050900	Unknown exceptions
        /// </exception>
        public DomainContactsResult GetContacts(string domain)
        {
            XDocument doc = new Query(_params)
              .AddParameter("DomainName", domain)
              .Execute("namecheap.domains.getContacts");

            var serializer = new XmlSerializer(typeof(DomainContactsResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainContactsResult").CreateReader())
            {
                return (DomainContactsResult)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns information about the requested domain.
        /// </summary>
        /// <param name="domain">Domain name for which domain information needs to be requested.</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 5019169	Unknown exceptions
        /// - 2030166	Domain is invalid
        /// - 4011103 - DomainName not Available; or UserName not Available; or Access denied
        /// </exception>
        public DomainInfoResult GetInfo(string domain)
        {
            XDocument doc = new Query(_params)
                .AddParameter("DomainName", domain)
                .Execute("namecheap.domains.getInfo");

            XElement root = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainGetInfoResult");

            return new DomainInfoResult()
            {
                ID = int.Parse(root.Attribute("ID").Value),
                OwnerName = root.Attribute("OwnerName").Value,
                IsOwner = bool.Parse(root.Attribute("IsOwner").Value),
                CreatedDate = root.Element(_ns + "DomainDetails").Element(_ns + "CreatedDate").Value.ParseNameCheapDate(),
                ExpiredDate = root.Element(_ns + "DomainDetails").Element(_ns + "ExpiredDate").Value.ParseNameCheapDate(),
                DnsProviderType = root.Element(_ns + "DnsDetails").Attribute("ProviderType").Value
            };
        }

        /// <summary>
        /// Returns a list of domains for the particular user.
        /// </summary>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// 5050169	Unknown exceptions
        /// </exception>
        public DomainListResult GetList()
        {
            XDocument doc = new Query(_params).Execute("namecheap.domains.getList");

            var serializer = new XmlSerializer(typeof(DomainListResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").CreateReader())
            {
                return (DomainListResult)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Gets the Registrar Lock status for the requested domain.
        /// </summary>
        /// <param name="domain">Domain name to get status for.</param>
        /// <returns>true if the domain is locked for registrar transfer, false if unlocked.</returns>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found
        /// - 2016166	Domain is not associated with your account
        /// - 3031510	Error response from provider when errorcount !=0
        /// - 3050900	Unknown error response from Enom
        /// - 5050900	Unknown exceptions
        /// </exception>
        public bool GetRegistrarLock(string domain)
        {
            XDocument doc = new Query(_params)
                .AddParameter("DomainName", domain)
                .Execute("namecheap.domains.getRegistrarLock");

            XElement root = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainGetRegistrarLockResult");
            return bool.Parse(root.Attribute("RegistrarLockStatus").Value);
        }

        /// <summary>
        /// Locks the domain for registrar transfer.
        /// </summary>
        /// <param name="domain">Domain name to lock.</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2015278	Invalid data specified for LockAction
        /// - 2019166	Domain not found
        /// - 2016166	Domain is not associated with your account
        /// - 3031510	Error from Enom when Errorcount != 0
        /// - 2030166	Edit permission for domain is not supported
        /// - 3050900	Unknown error response from Enom
        /// - 5050900	Unknown exceptions
        /// </exception>
        public void SetRegistrarLock(string domain)
        {
            new Query(_params)
                .AddParameter("DomainName", domain)
                .Execute("namecheap.domains.setRegistrarLock");
        }

        /// <summary>
        /// Unlocks (opens) the domain for registrar transfer.
        /// </summary>
        /// <param name="domain">Domain name to unlock.</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2015278	Invalid data specified for LockAction
        /// - 2019166	Domain not found
        /// - 2016166	Domain is not associated with your account
        /// - 3031510	Error from Enom when Errorcount != 0
        /// - 2030166	Edit permission for domain is not supported
        /// - 3050900	Unknown error response from Enom
        /// - 5050900	Unknown exceptions
        /// </exception>
        public void SetRegistrarUnlock(string domain)
        {
            new Query(_params)
                .AddParameter("DomainName", domain)
                .AddParameter("LockAction", "UNLOCK")
                .Execute("namecheap.domains.setRegistrarLock");
        }

        /// <summary>
        /// Returns a list of TLD - top level domains.
        /// </summary>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2011166	UserName is invalid
        /// - 3050900	Unknown response from provider
        /// </exception>
        public TldListResult GetTldList()
        {
            XDocument doc = new Query(_params).Execute("namecheap.domains.getTldList");

            var serializer = new XmlSerializer(typeof(TldListResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "Tlds").CreateReader())
            {
                return (TldListResult)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Renews an expiring domain.
        /// </summary>
        /// <param name="domain">Domain name to renew.</param>
        /// <param name="years">Number of years to renew.</param>
        /// <returns>information about the renewal, such as the charged amount, or the order Id.</returns>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2033409	Possibly a logical error at the authentication phase. The order chargeable for the Username is not found.
        /// - 2011170	Validation error from PromotionCode
        /// - 2011280	TLD is invalid
        /// - 2528166	Order creation failed
        /// - 2020166	Domain has expired. Please reactivate your domain.
        /// - 3028166	Failed to renew, error from Enom
        /// - 3031510	Error from Enom ( Errcount != 0 )
        /// - 3050900	Unknown error from Enom
        /// - 2016166	Domain is not associated with your account
        /// - 4024167	Failed to update years for your domain
        /// - 4023166	Error occurred during the domain renewal
        /// - 4022337	Error in refunding funds
        /// - 2015170	Promotion code is not allowed for premium domains
        /// - 2015167	Premium domain can be renewed for 1 year only
        /// - 2015610	Premium prices cannot be zero for premium domains
        /// - 2515623	You are trying to renew a premium domain. Premium price should be added to request to renew the premium domain.
        /// - 2511623	Domain name is not premium
        /// - 2515610	Premium price is incorrect. It should be (premium renewal price value).
        /// </exception>
        public DomainRenewResult Renew(string domain, int years)
        {
            XDocument doc = new Query(_params)
             .AddParameter("DomainName", domain)
             .AddParameter("Years", years.ToString())
             .Execute("namecheap.domains.renew");

            var serializer = new XmlSerializer(typeof(DomainRenewResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainRenewResult").CreateReader())
            {
                return (DomainRenewResult)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Reactivates an expired domain.
        /// </summary>
        /// <param name="domain">Domain to reactivate.</param>
        /// <returns>information about the renewal, such as the charged amount, or the order Id.</returns>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2033409	Possibly a logical error at the authentication phase. The order chargeable for the Username is not found.
        /// - 2019166	Domain not found
        /// - 2030166	Edit permission for the domain is not supported
        /// - 2011170	Promotion code is invalid
        /// - 2011280	TLD is invalid
        /// - 2528166	Order creation failed
        /// - 3024510	Error response from Enom while updating the domain
        /// - 3050511	Unknown error response from Enom
        /// - 2020166	Domain does not meet the expiration date for reactivation
        /// - 2016166	Domain is not associated with your account
        /// - 5050900	Unhandled exceptions
        /// - 4024166	Failed to update the domain in your account
        /// - 2015170	Promotion code is not allowed for premium domains
        /// - 2015167	Premium domain can be reactivated for 1 year only
        /// - 2015610	Premium prices cannot be zero for premium domains
        /// - 2515623	You are trying to reactivate a premium domain. Premium price should be added to the request to reactivate the premium domain.
        /// - 2511623	Domain name is not premium
        /// - 2515610	Premium price is incorrect. It should be (premium renewal price value).
        /// </exception>
        public DomainReactivateResult Reactivate(string domain)
        {
            XDocument doc = new Query(_params)
             .AddParameter("DomainName", domain)
             .Execute("namecheap.domains.reActivate");

            var serializer = new XmlSerializer(typeof(DomainReactivateResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainReactivateResult").CreateReader())
            {
                return (DomainReactivateResult)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Sets contact information for the requested domain.
        /// </summary>
        /// <param name="contacts">
        /// The contact information to be set.
        /// All 4 parameters, Registrant, Tech, Admin, and Aux Billig
        /// need to be present. The required fields for each address
        /// are: FirstName, LastName, Address1, StateProvince,
        /// PostalCode, Country, Phone, and EmailAddress.</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found
        /// - 2030166	Edit permission for domain is not supported
        /// - 2010324	Registrant contacts such as firstname, lastname etc. are missing
        /// - 2010325	Tech contacts such as firstname, lastname etc. are missing
        /// - 2010326	Admin contacts such as firstname, lastname etc. are missing
        /// - 2015182	The contact phone is invalid. The phone number format is +NNN.NNNNNNNNNN
        /// - 2010327	AuxBilling contacts such as firstname, lastname etc. are missing
        /// - 2016166	Domain is not associated with your account
        /// - 2011280	Cannot see the contact information for your TLD
        /// - 4022323	Error retrieving domain Contacts
        /// - 2011323	Error retrieving domain Contacts from Enom (invalid errors)
        /// - 3031510	Error from Enom when error count != 0
        /// - 3050900	Unknown error from Enom
        /// </exception>
        public void SetContacts(DomainContactsRequest contacts)
        {
            var query = new Query(_params);

            foreach (var item in GetNamesAndValuesFromProperties(contacts))
                query.AddParameter(item.Key, item.Value);

            XDocument doc = query.Execute("namecheap.domains.setContacts");
        }

        private Dictionary<string, string> GetNamesAndValuesFromProperties(object obj)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();

            foreach (var property in obj.GetType().GetProperties())
            {
                object value = property.GetValue(obj, null);

                if (value is ContactInformation)
                {
                    foreach (var cProperty in value.GetType().GetProperties())
                    {
                        var cValue = cProperty.GetValue(value, null);

                        if (cValue != null)
                            queryParams.Add(property.Name + cProperty.Name, cValue.ToString());
                    }
                }
                else if (value != null)
                    queryParams.Add(property.Name, value.ToString());
            }

            return queryParams;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NameCheap
{
    public class DomainsApi
    {
        private readonly XNamespace _ns = XNamespace.Get("http://api.namecheap.com/xml.response");
        private readonly GlobalParameters _params;

        internal DomainsApi(GlobalParameters globalParams)
        {
            _params = globalParams;
        }

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

        public DomainListResult GetList()
        {
            XDocument doc = new Query(_params).Execute("namecheap.domains.getList");

            var serializer = new XmlSerializer(typeof(DomainListResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").CreateReader())
            {
                return (DomainListResult)serializer.Deserialize(reader);
            }
        }

        public bool GetRegistrarLock(string domain)
        {
            XDocument doc = new Query(_params)
                .AddParameter("DomainName", domain)
                .Execute("namecheap.domains.getRegistrarLock");

            XElement root = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainGetRegistrarLockResult");
            return bool.Parse(root.Attribute("RegistrarLockStatus").Value);
        }

        public void SetRegistrarLock(string domain)
        {
            new Query(_params)
                .AddParameter("DomainName", domain)
                .Execute("namecheap.domains.setRegistrarLock");
        }

        public TldListResult GetTldList()
        {
            XDocument doc = new Query(_params).Execute("namecheap.domains.getTldList");

            var serializer = new XmlSerializer(typeof(TldListResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "Tlds").CreateReader())
            {
                return (TldListResult)serializer.Deserialize(reader);
            }
        }

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

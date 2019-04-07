using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NameCheap
{
    public class DnsApi
    {
        private readonly XNamespace _ns = XNamespace.Get("http://api.namecheap.com/xml.response");
        private readonly GlobalParameters _params;

        internal DnsApi(GlobalParameters globalParams)
        {
            _params = globalParams;
        }

        public void SetHosts(DnsHostsRequest request)
        {
            var query = new Query(_params);
            query.AddParameter("SLD", request.SLD);
            query.AddParameter("TLD", request.TLD);

            for (int i = 0; i < request.HostEntries.Length; i++)
            {
                query.AddParameter("HostName" + (i + 1), request.HostEntries[i].HostName);
                query.AddParameter("Address" + (i + 1), request.HostEntries[i].Address);
                query.AddParameter("MxPref" + (i + 1), request.HostEntries[i].MxPref);
                query.AddParameter("RecordType" + (i + 1), Enum.GetName(typeof(RecordType), request.HostEntries[i].RecordType));

                if (!string.IsNullOrEmpty(request.HostEntries[i].Ttl))
                    query.AddParameter("TTL" + (i + 1), request.HostEntries[i].Ttl);
            }

            XDocument doc = query.Execute("namecheap.domains.dns.setHosts");
        }

        public DnsHostResult GetHosts(string sld, string tld)
        {
            var query = new Query(_params)
                .AddParameter("SLD", sld)
                .AddParameter("TLD", tld);

            XDocument doc = query.Execute("namecheap.domains.dns.getHosts");

            var serializer = new XmlSerializer(typeof(DnsHostResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainDNSGetHostsResult").CreateReader())
            {
                return (DnsHostResult)serializer.Deserialize(reader);
            }
        }

        public DnsEmailForwardingResult GetEmailForwarding(string domain)
        {
            var query = new Query(_params);
            query.AddParameter("DomainName", domain);

            XDocument doc = query.Execute("namecheap.domains.dns.getEmailForwarding");

            var serializer = new XmlSerializer(typeof(DnsEmailForwardingResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainDNSGetEmailForwardingResult").CreateReader())
            {
                return (DnsEmailForwardingResult)serializer.Deserialize(reader);
            }
        }

        public void SetEmailForwarding(string domain, EmailForwarding[] request)
        {
            var query = new Query(_params)
                .AddParameter("DomainName", domain);

            for (int i = 0; i < request.Length; i++)
            {
                query.AddParameter("MailBox" + (i + 1), request[i].MailBox);
                query.AddParameter("ForwardTo" + (i + 1), request[i].ForwardTo);
            }

            query.Execute("namecheap.domains.dns.setEmailForwarding");
        }

        /// <summary>
        /// Deletes all the email forwarding for a domain.
        /// </summary>
        /// <param name="domain">The domain for which to delete the forwards.</param>
        public void DeleteAllEmailForwarding(string domain)
        {
            SetEmailForwarding(domain, new EmailForwarding[0]);
        }

        public DnsListResult GetList(string sld, string tld)
        {
            var query = new Query(_params)
                 .AddParameter("SLD", sld)
                 .AddParameter("TLD", tld);

            XDocument doc = query.Execute("namecheap.domains.dns.getList");

            var serializer = new XmlSerializer(typeof(DnsListResult), _ns.NamespaceName);

            using (var reader = doc.Root.Element(_ns + "CommandResponse").Element(_ns + "DomainDNSGetListResult").CreateReader())
            {
                return (DnsListResult)serializer.Deserialize(reader);
            }
        }

        public void SetCustom(string sld, string tld, params string[] nameservers)
        {
            var query = new Query(_params)
                 .AddParameter("SLD", sld)
                 .AddParameter("TLD", tld)
                 .AddParameter("Nameservers", string.Join(",", nameservers));

            query.Execute("namecheap.domains.dns.setCustom");
        }

        public void SetDefault(string sld, string tld)
        {
            var query = new Query(_params)
                 .AddParameter("SLD", sld)
                 .AddParameter("TLD", tld);

            query.Execute("namecheap.domains.dns.setDefault");
        }
    }
}

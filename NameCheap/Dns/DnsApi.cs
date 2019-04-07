using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NameCheap
{
    /// <summary>Set of functions used to manipulate DNS records: hosts, name servers, and even email forwarding.</summary>
    public class DnsApi
    {
        private readonly XNamespace _ns = XNamespace.Get("http://api.namecheap.com/xml.response");
        private readonly GlobalParameters _params;

        internal DnsApi(GlobalParameters globalParams)
        {
            _params = globalParams;
        }

        /// <summary>
        /// Sets DNS host records settings for the requested domain.
        /// </summary>
        /// <param name="secondLevelDomain">The second level domain, SLD, of the domain for which to set the hosts (the abc in abc.xyz).</param>
        /// <param name="topLevelDomain">The top-level domain, TLD, of the domain for which to set the hosts (the xyz of abc.xyz).</param>
        /// <param name="hostEntries">The list of hosts entries to set.
        /// These need to obey their respective DNS record type rules"
        /// correct IP address for A-records,
        /// domain (not IP) for CNAME, etc</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found
        /// - 2016166	Domain is not associated with your account
        /// - 2030166	Edit permission for domain is not supported
        /// - 3013288, 4013288	Too many records
        /// - 3031510	Error From Enom when Errorcount != 0
        /// - 3050900	Unknown error from Enom
        /// - 4022288	Unable to get nameserver list
        /// </exception>
        public void SetHosts(string secondLevelDomain, string topLevelDomain, HostEntry[] hostEntries)
        {
            var query = new Query(_params);
            query.AddParameter("SLD", secondLevelDomain);
            query.AddParameter("TLD", topLevelDomain);

            for (int i = 0; i < hostEntries.Length; i++)
            {
                query.AddParameter("HostName" + (i + 1), hostEntries[i].HostName);
                query.AddParameter("Address" + (i + 1), hostEntries[i].Address);
                query.AddParameter("MxPref" + (i + 1), hostEntries[i].MxPref);
                query.AddParameter("RecordType" + (i + 1), Enum.GetName(typeof(RecordType), hostEntries[i].RecordType));

                if (!string.IsNullOrEmpty(hostEntries[i].Ttl))
                    query.AddParameter("TTL" + (i + 1), hostEntries[i].Ttl);
            }

            XDocument doc = query.Execute("namecheap.domains.dns.setHosts");
        }

        /// <summary>
        /// Retrieves DNS host record settings for the requested domain..
        /// </summary>
        /// <param name="sld">The second level domain, SLD, of the domain for which to get the hosts (the abc in abc.xyz).</param>
        /// <param name="tld">The top-level domain, TLD, of the domain for which to get the hosts (the xyz of abc.xyz).</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found
        /// - 2030166	Edit permission for domain is not supported
        /// - 2030288	Cannot complete this command as this domain is not using proper DNS servers
        /// - 4023330	Unable to get DNS hosts from list
        /// - 3031510	Error From Enom when Errorcount != 0
        /// - 3050900	Unknown error from Enom
        /// - 3011288	Invalid name server specified
        /// - 5050900	Unhandled Exceptions
        /// </exception>
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

        /// <summary>
        /// Gets email forwarding settings for the requested domain.
        /// </summary>
        /// <param name="domain">the domain for which to get forwarding settings.</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found
        /// - 2030166	Edit permission for domain is not supported
        /// - 2030288	Cannot complete this command as this domain is not using proper DNS servers
        /// - 3031510	Error From Enom when Errorcount != 0
        /// - 3050900	Unknown error from Enom
        /// - 4022328	Unable to get EmailForwarding records from database
        /// - 3011288	Invalid nameserver
        /// - 5050900	Unhandled Exceptions
        /// </exception>
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

        /// <summary>
        /// Sets email forwarding for a domain name.
        /// </summary>
        /// <param name="domain">The domain for which to set email forwarding.</param>
        /// <param name="request">The entire list of forwards to set up.</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found 
        /// - 2016166 Domain is not associated with your account
        /// - 2030288 Cannot complete this command as this domain is not using proper DNS servers
        /// - 2030166 Edit Permission for domain is not supported
        /// - 3013288 Too many records
        /// - 3031510 Error From Enom when Errorcount != 0
        /// - 3050900 Unknown error from Enom
        /// - 4022228 Unable to get nameserver list
        /// </exception>
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
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found 
        /// - 2016166 Domain is not associated with your account
        /// - 2030288 Cannot complete this command as this domain is not using proper DNS servers
        /// - 2030166 Edit Permission for domain is not supported
        /// - 3031510 Error From Enom when Errorcount != 0
        /// - 3050900 Unknown error from Enom
        /// - 4022228 Unable to get nameserver list
        /// </exception>
        public void DeleteAllEmailForwarding(string domain)
        {
            SetEmailForwarding(domain, new EmailForwarding[0]);
        }

        /// <summary>
        /// Gets a list of DNS servers associated with the requested domain.
        /// </summary>
        /// <param name="sld">The second level domain, SLD, of the domain for which to get the list of name servers (the abc in abc.xyz).</param>
        /// <param name="tld">The top-level domain, TLD, of the domain for which to get the list of name servers (the xyz of abc.xyz).</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found
        /// - 2016166	Domain is not associated with your account
        /// - 3031510	Error From Enom when Errorcount != 0
        /// - 3050900	Unknown error from Enom
        /// - 4022288	Unable to get nameserver list
        /// </exception>
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

        /// <summary>
        /// Sets domain to use custom DNS servers.
        /// </summary>
        /// <param name="sld">The second level domain, SLD, of the domain for which to set the list of name servers (the abc in abc.xyz).</param>
        /// <param name="tld">The top-level domain, TLD, of the domain for which to set the list of name servers (the xyz of abc.xyz).</param>
        /// <param name="nameservers">IP address of the custom domain name servers.</param>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found
        /// - 2016166	Domain is not associated with your account
        /// - 2030166	Edit permission for domain is not supported
        /// - 3031510	Error From Enom when Errorcount != 0
        /// - 3050900	Unknown error from Enom
        /// - 4022288	Unable to get nameserver list
        /// </exception>
        public void SetCustom(string sld, string tld, params string[] nameservers)
        {
            var query = new Query(_params)
                 .AddParameter("SLD", sld)
                 .AddParameter("TLD", tld)
                 .AddParameter("Nameservers", string.Join(",", nameservers));

            query.Execute("namecheap.domains.dns.setCustom");
        }

        /// <summary>
        /// Sets domain to use NameCheap's default DNS servers.
        /// </summary>
        /// <param name="sld">The second level domain, SLD, of the domain for which to use the default name servers (the abc in abc.xyz).</param>
        /// <param name="tld">The top-level domain, TLD, of the domain for which to use the default name servers (the xyz of abc.xyz).</param>
        /// <remarks>
        /// Required for free services like Host record management, URL forwarding, email forwarding, dynamic dns and other value added services.
        /// </remarks>
        /// <exception cref="ApplicationException">
        /// Exception when the following problems are encountered:
        /// - 2019166	Domain not found
        /// - 2016166	Domain is not associated with your account
        /// - 2030166	Edit permission for domain is not supported
        /// - 3013288	Too many records
        /// - 3031510	Error From Enom when Errorcount != 0
        /// - 3050900	Unknown error from Enom
        /// - 4022288	Unable to get nameserver list
        /// </exception>
        public void SetDefault(string sld, string tld)
        {
            var query = new Query(_params)
                 .AddParameter("SLD", sld)
                 .AddParameter("TLD", tld);

            query.Execute("namecheap.domains.dns.setDefault");
        }
    }
}

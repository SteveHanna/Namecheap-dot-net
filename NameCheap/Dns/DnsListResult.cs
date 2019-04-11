using System.Collections.Generic;
using System.Xml.Serialization;

namespace NameCheap
{
    [XmlRoot("DomainDNSGetListResult")]
    public class DnsListResult
    {
        [XmlAttribute("IsUsingOurDNS")]
        public bool IsUsingOurDns { get; set; }

        [XmlElement("Nameserver")]
        public List<string> NameServers { get; set; }
    }
}

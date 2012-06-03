using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NameCheap
{
    [XmlRoot("DomainDNSGetHostsResult")]
    public class DnsHostResult
    {
        [XmlAttribute("IsUsingOurDNS")]
        public bool IsUsingOurDns { get; set; }

        [XmlElement("host")]
        public HostEntry[] HostEntries { get; set; }
    }
}

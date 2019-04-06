using System.Collections.Generic;
using System.Xml.Serialization;

namespace NameCheap
{
    [XmlRoot("DomainDNSGetEmailForwardingResult")]
    public class DnsEmailForwardingResult
    {
        [XmlElement("Forward")]
        public List<EmailForwarding> Emails { get; set; }
    }
}

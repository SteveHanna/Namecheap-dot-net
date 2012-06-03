using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NameCheap
{
    [XmlRoot("CommandResponse")]
    public class DomainListResult
    {
        [XmlArray("DomainGetListResult")]
        [XmlArrayItem("Domain", typeof(Domain))]
        public Domain[] Domains { get; set; }
    }
}

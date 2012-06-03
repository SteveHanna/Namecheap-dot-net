using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NameCheap
{
    [XmlRoot("DomainReactivateResult")]
    public class DomainReactivateResult
    {
        [XmlAttribute]
        public string DomainName { get; set; }
        [XmlAttribute]
        public int DomainID { get; set; }
        [XmlAttribute]
        public bool IsSuccess { get; set; }
        [XmlAttribute]
        public int OrderID { get; set; }
        [XmlAttribute]
        public int TransactionID { get; set; }
        [XmlAttribute]
        public double ChargedAmount { get; set; }
    }
}

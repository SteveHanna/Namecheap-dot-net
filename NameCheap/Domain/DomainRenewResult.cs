using System.Xml.Serialization;

namespace NameCheap
{
    [XmlRoot("DomainRenewResult")]
    public class DomainRenewResult
    {
        [XmlAttribute]
        public string DomainName { get; set; }
        [XmlAttribute]
        public int DomainID { get; set; }
        [XmlAttribute]
        public bool Renew { get; set; }
        [XmlAttribute]
        public int OrderID { get; set; }
        [XmlAttribute]
        public int TransactionID { get; set; }
        [XmlAttribute]
        public double ChargedAmount { get; set; }
    }
}

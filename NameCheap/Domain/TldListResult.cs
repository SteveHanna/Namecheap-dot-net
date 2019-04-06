using System.Xml.Serialization;

namespace NameCheap
{
    [XmlRoot("Tlds")]
    public class TldListResult
    {
        [XmlElement("Tld")]
        public Tld[] Tlds { get; set; }
    }
}

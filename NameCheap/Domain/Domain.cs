using System;
using System.Xml.Serialization;

namespace NameCheap
{
    public class Domain
    {
        [XmlAttribute]
        internal int ID { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string User { get; set; }
        [XmlIgnore]
        public DateTime Created { get; set; }
        [XmlIgnore]
        public DateTime Expires { get; set; }
        [XmlAttribute("Created")]
        public string CreatedDateString { set { this.Created = value.ParseNameCheapDate(); } }
        [XmlAttribute("Expires")]
        public string ExpiresDateString { set { this.Expires = value.ParseNameCheapDate(); } }
        [XmlAttribute]
        public bool IsExpired { get; set; }
        [XmlAttribute]
        public bool IsLocked { get; set; }
        [XmlAttribute]
        public bool AutoRenew { get; set; }
        [XmlAttribute]
        public string WhoisGuard { get; set; }
    }
}

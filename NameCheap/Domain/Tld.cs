using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NameCheap
{
    public class Tld
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public string LongName { get; set; }
        [XmlAttribute]
        public bool NonRealTime { get; set; }
        [XmlAttribute]
        public int MinRegisterYears { get; set; }
        [XmlAttribute]
        public int MaxRegisterYears { get; set; }
        [XmlAttribute]
        public int MinRenewYears { get; set; }
        [XmlAttribute]
        public int MaxRenewYears { get; set; }
        [XmlAttribute]
        public int MinTransferYears { get; set; }
        [XmlAttribute]
        public int MaxTransferYears { get; set; }
        [XmlAttribute]
        public bool IsApiRegisterable { get; set; }
        [XmlAttribute]
        public bool IsApiRenewable { get; set; }
        [XmlAttribute]
        public bool IsApiTransferable { get; set; }
        [XmlAttribute]
        public bool IsEppRequired { get; set; }
        [XmlAttribute]
        public bool IsDisableModContact { get; set; }
        [XmlAttribute]
        public bool IsDisableWGAllot { get; set; }
        [XmlAttribute]
        public bool IsIncludeInExtendedSearchOnly { get; set; }
        [XmlAttribute]
        public int SequenceNumber { get; set; }
        [XmlAttribute]
        public bool IsSupportsIDN { get; set; }
        [XmlAttribute]
        public string Category { get; set; }
    }
}

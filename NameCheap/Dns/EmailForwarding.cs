using System.Xml.Serialization;

namespace NameCheap
{
    public class EmailForwarding
    {
        [XmlAttribute("mailbox")]
        public string MailBox { get; set; }
        [XmlText]
        public string ForwardTo { get; set; }
    }
}

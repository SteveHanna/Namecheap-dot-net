using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NameCheap
{
    public class DomainInfoResult
    {
        public int ID { get; set; }
        public bool IsOwner { get; set; }
        public string OwnerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string DnsProviderType { get; set; }
    }
}

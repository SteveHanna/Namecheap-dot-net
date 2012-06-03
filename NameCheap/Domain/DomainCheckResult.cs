using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NameCheap
{
    public class DomainCheckResult
    {
        public string DomainName { get; set; }
        public bool IsAvailable { get; set; }
    }
}

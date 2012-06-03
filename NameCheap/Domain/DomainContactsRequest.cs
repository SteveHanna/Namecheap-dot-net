using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NameCheap
{
    public class DomainContactsRequest
    {
        public string DomainName { get; set; }
        public ContactInformation Registrant { get; set; }
        public ContactInformation Tech { get; set; }
        public ContactInformation Admin { get; set; }
        public ContactInformation AuxBilling { get; set; }
    }
}

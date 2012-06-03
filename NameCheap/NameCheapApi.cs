using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.Xml.Serialization;
using System.Globalization;

namespace NameCheap
{
    public class NameCheapApi
    {
        private readonly GlobalParameters _params;

        public NameCheapApi(string username, string apiUser, string apiKey, string clientIp, bool isSandbox = false)
        {
            _params = new GlobalParameters()
            {
                ApiKey = apiKey,
                ApiUser = apiUser,
                CLientIp = clientIp,
                IsSandBox = isSandbox,
                UserName = username
            };
        }

        public DomainsApi Domains { get { return new DomainsApi(_params); } }
        public DnsApi Dns { get { return new DnsApi(_params); } }
    }
}

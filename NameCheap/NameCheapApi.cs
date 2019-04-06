using System;
using System.Net;
using System.Net.Sockets;

namespace NameCheap
{
    public class NameCheapApi
    {
        private readonly GlobalParameters _params;

        public NameCheapApi(string username, string apiUser, string apiKey, string clientIp, bool isSandbox = false)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrWhiteSpace(apiUser))
            {
                throw new ArgumentNullException(nameof(apiUser));
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            if (string.IsNullOrWhiteSpace(clientIp))
            {
                throw new ArgumentNullException(nameof(clientIp));
            }
            else
            {
                if (IPAddress.TryParse(clientIp, out var ip))
                {
                    if (ip.AddressFamily != AddressFamily.InterNetwork)
                    {
                        throw new ArgumentException($"Client IP {clientIp} is not a valid IPv4 address.", nameof(clientIp));
                    }
                }
                else
                {
                    throw new ArgumentException($"{clientIp} does not seem a valid IP address.", nameof(clientIp));
                }
            }

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

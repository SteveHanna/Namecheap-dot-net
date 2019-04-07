using System;
using System.Net;
using System.Net.Sockets;

namespace NameCheap
{
    /// <summary>
    /// Collection of API methods for programmatically performing operations against NameCheap domains.
    /// </summary>
    public class NameCheapApi
    {
        private readonly GlobalParameters _params;
        private readonly Lazy<DnsApi> _dnsApi;
        private readonly Lazy<DomainsApi> _domainsApi;

        /// <summary>
        /// Creates a NameCheap API instance configured with the appropriate credentials.
        /// All parameters are required.
        /// </summary>
        /// <param name="username">The Username on which a command is executed. Generally, the values of ApiUser and UserName parameters are the same.</param>
        /// <param name="apiUser">Username required to access the API</param>
        /// <param name="apiKey">Password required used to access the API</param>
        /// <param name="clientIp">An IP address of the server from which our system receives API calls (only IPv4 can be used).</param>
        /// <param name="isSandbox">Whether to execute the commands against the sandbox API or the live API.</param>
        /// <remarks>
        /// See the '$/docs/running_tests.md' on how to set up a Sandbox account in order
        /// to test the operations of the API before going live.
        /// </remarks>
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

            _dnsApi = new Lazy<DnsApi>(() => new DnsApi(_params));
            _domainsApi = new Lazy<DomainsApi>(() => new DomainsApi(_params));
        }

        /// <summary>Gets the set of functions responsible for domain management (creating, info, renewal, contacts, etc).</summary>
        public DomainsApi Domains => _domainsApi.Value;

        /// <summary>Gets the set of functions used to manipulate DNS records (hosts, name servers, and even email forwarding).</summary>
        public DnsApi Dns => _dnsApi.Value;
    }
}

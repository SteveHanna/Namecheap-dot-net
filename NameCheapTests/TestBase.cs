using System;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NameCheap;

namespace NameCheapTests
{
    [TestClass]
    public abstract class TestBase
    {
        protected static readonly Lazy<string> _apiUser;
        protected static readonly Lazy<string> _apiKey;
        protected static readonly Lazy<string> _clientIp;

        protected NameCheapApi _api = new NameCheapApi(_apiUser.Value, _apiUser.Value, _apiKey.Value, _clientIp.Value, isSandbox: true);

        // Domain used (and re-used for testing) - changing this value could have adverse effects to the test suite
        protected const string _domainName = "aeb80572-9b17-4ac9-8c24-048d2991119b.com"; // eaba62ff-e035-417a-8760-bd2d33972a25.com";
        protected const string TestUserFirstName = "TestFirstName";
        protected const string TestUserLastName = "TestLastName";

        // whether domain exists - null
        private static bool? _domainExists = null;

        static TestBase()
        {
            var config = new Lazy<IConfiguration>(() => LoadSettings());
            _apiUser = new Lazy<string>(() => config.Value.GetSection("apiUser").Value);
            _apiKey = new Lazy<string>(() => config.Value.GetSection("apiKey").Value);
            _clientIp = new Lazy<string>(() => config.Value.GetSection("clientIp").Value);
        }

        [AssemblyInitialize]
        public static void BeforeAllTestsInTheAssembly(TestContext context)
        {
            // this is where all the very expensive code goes.
            EnsureTestDomain();
        }

        protected static void EnsureTestDomain()
        {
            if (_domainExists.HasValue && _domainExists == true)
            {
                return;
            }

            if (!_domainExists.HasValue)
            {
                try
                {
                    var api = new NameCheapApi(_apiUser.Value, _apiUser.Value, _apiKey.Value, _clientIp.Value, isSandbox: true);
                    DomainInfoResult info = api.Domains.GetInfo(_domainName);
                    _domainExists = true;
                }
                catch (ApplicationException) // TODO: where (e.ErrorCode == 2030166)
                {
                    // likely the domain doesn't exist
                    _domainExists = false;
                }
            }

            if (_domainExists == false)
            {
                _ = CreateTestDomain(); // might throw
            }
        }

        private static DomainCreateResult CreateTestDomain()
        {
            // TODO: use different names for each type of contact to distinguish between them in testing
            var contact = new ContactInformation()
            {
                Address1 = "1 never never land",
                City = "New York",
                Country = "US",
                EmailAddress = "noreply@example.com",
                FirstName = TestUserFirstName,
                LastName = TestUserLastName,
                Phone = "+011.5555555555",
                PostalCode = "l5Z5Z5",
                StateProvince = "California"
            };

            var api = new NameCheapApi(_apiUser.Value, _apiUser.Value, _apiKey.Value, _clientIp.Value, isSandbox: true);
            var domain = api.Domains.Create(new DomainCreateRequest()
            {
                DomainName = _domainName,
                Admin = contact, 
                AuxBilling = contact,
                Registrant = contact,
                Tech = contact,
                Years = 1
            });

            return domain;
        }

        private static IConfiguration LoadSettings()
        {
            // home path is:
            // -Windows: c:\user\<user>\Documents 
            // - Unix: $HOME aka ~
            //         - /home/<user> on Linux
            //         - /Users/<user> on macOS
            var homePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            IConfiguration appSettings = new ConfigurationBuilder()
                .SetBasePath(homePath)
                .AddJsonFile("namecheapdotnet-settings.json", optional: true)
                .AddEnvironmentVariables("NAMECHEAPDOTNET_")
                .Build();

            return appSettings;
        }
    }
}

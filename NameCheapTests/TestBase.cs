using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NameCheapTests
{
    public class TestBase
    {
        protected static readonly Lazy<string> _apiUser;
        protected static readonly Lazy<string> _apiKey;
        protected static readonly Lazy<string> _clientIp;

        static TestBase()
        {
            var config = new Lazy<IConfiguration>(() => LoadSettings());
            _apiUser = new Lazy<string>(() => config.Value.GetSection("apiUser").Value);
            _apiKey = new Lazy<string>(() => config.Value.GetSection("apiKey").Value);
            _clientIp = new Lazy<string>(() => config.Value.GetSection("clientIp").Value);
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
                .AddJsonFile("namecheapdotnet-settings.json")
                .AddEnvironmentVariables("NAMECHEAPDOTNET_")
                .Build();

            return appSettings;
        }
    }
}

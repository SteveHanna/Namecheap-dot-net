using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;

namespace NameCheap
{
    internal class Query
    {
        private readonly XNamespace _ns = XNamespace.Get("http://api.namecheap.com/xml.response");
        private readonly GlobalParameters _globals;
        private List<KeyValuePair<string, string>> _parameters = new List<KeyValuePair<string, string>>();

        internal Query(GlobalParameters globals)
        {
            if (globals == null)
                throw new ArgumentNullException("globals");

            _globals = globals;
        }

        internal Query AddParameter(string key, string value)
        {
            _parameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        internal XDocument Execute(string command)
        {
            StringBuilder url = new StringBuilder();
            url.Append(_globals.IsSandBox
                ? "https://api.sandbox.namecheap.com/xml.response?"
                : "https://api.namecheap.com/xml.response?");
            url.Append("Command=").Append(command)
                .Append("&ApiUser=").Append(_globals.ApiUser)
                .Append("&UserName=").Append(_globals.UserName)
                .Append("&ApiKey=").Append(_globals.ApiKey)
                .Append("&ClientIp=").Append(_globals.CLientIp);

            foreach (KeyValuePair<string, string> param in _parameters)
                url.Append("&").Append(param.Key).Append("=").Append(param.Value);

            string content = new WebClient().DownloadString(url.ToString());
            XDocument doc = XDocument.Parse(content);

            if (doc.Root?.Attribute("Status") is null)
            {
                throw new ApplicationException(
                    $"Received a null root or a missing STATUS attribute. XML document: {content}");
            }

            if (doc.Root.Attribute("Status").Value.Equals("ERROR", StringComparison.OrdinalIgnoreCase))
                try
                {
                    XNamespace ns = doc.Root.Element("Errors") != null ? "" : _ns;
                    throw new ApplicationException(string.Join(",",
                        doc.Root.Element(ns + "Errors")?.Elements(ns + "Error").Select(o => o.Value).ToArray() ??
                        Array.Empty<string>()));
                }
                catch (NullReferenceException)
                {
                    throw new ApplicationException($"Error finding errors in XML doc: {content}");
                }

            return doc;
        }
    }
}
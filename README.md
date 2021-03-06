# Namecheap-dot-net

A .NET wrapper for the [Namecheap API](https://www.namecheap.com/support/api/api.aspx), with support for the [NameCheap Sandbox](https;//www.sandbox.namecheap.com/support/api/api.aspx).

[![NuGet version (NameCheapDotNet)](https://img.shields.io/nuget/v/NameCheapDotNet.svg?style=flat-square)](https://www.nuget.org/packages/NameCheapDotNet/)

## Sample Code

### Check the availability of several domain names

```c#
var api = new NameCheapApi("{username}", "{apiUser}", "{apiKey}", "{clientIp}", isSandbox: false);

var domains = api.Domains.AreAvailable("google.com", "ANewDomainName");

foreach (var domain in domains)
    Console.WriteLine(domain.DomainName + ": " + domain.IsAvailable);
```

### Set DNS host records for a given domain in the Sandbox

```c#
var api = new NameCheapApi("{username}", "{apiUser}", "{apiKey}", "{clientIp}", isSandbox: true);
 
api.Dns.SetHosts(
    new DnsHostsRequest
    {
        SLD = "YourDomainName",
        TLD = "com",
        HostEntries = new HostEntry[] { new HostEntry() { 
            Address = "192.168.1.1",
            HostName = "@",
            RecordType =  RecordType.A
        } }
    });
```
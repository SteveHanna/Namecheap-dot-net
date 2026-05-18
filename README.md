# Namecheap-dot-net

A .NET wrapper for the [Namecheap API](https://www.namecheap.com/support/api/api.aspx), with support for the [NameCheap Sandbox](https;//www.sandbox.namecheap.com/support/api/api.aspx).

[![NuGet version (NameCheapDotNet)](https://img.shields.io/nuget/v/NameCheapDotNet.svg?style=flat-square)](https://www.nuget.org/packages/NameCheapDotNet/)

## Using the Library

To use the library you will need to 
[enable API access on your NameCheap account](https://www.sandbox.namecheap.com/support/api/intro/),
and retrieve:

- the `username` and `apiUser` -- these are both your NameCheap login
- the `apiKey` -- retrieve this from your profile after [enabling API access](https://www.sandbox.namecheap.com/support/api/intro/)
- your current public IP, or the IP of the machine you'll run the library from.
  *Note* that this IP has to be added to the access list in your NameCheap profile.

### Sample Code

#### Check the availability of several domain names

```c#
var api = new NameCheapApi("{username}", "{apiUser}", "{apiKey}", "{clientIp}", isSandbox: false);

var domains = api.Domains.AreAvailable("google.com", "ANewDomainName");

foreach (var domain in domains)
    Console.WriteLine(domain.DomainName + ": " + domain.IsAvailable);
```

#### Set DNS host records for a given domain in the Sandbox

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

## Developing

For development, it's highly recommeded to create an account
on `sandbox.namecheap.com` and
[enable API access on your NameCheap sandbox account](https://www.sandbox.namecheap.com/support/api/intro/).  
Also create a domain; to avoid conflict, create an UUID domain.

Then copy the `namecheap-sandbox.json` to your _Documents_ folder on macOS and Windows,
and to your `$HOME` directory on Linux, and change the file with the appropriate
values from your sandbox account.

This should allow you to run the tests.


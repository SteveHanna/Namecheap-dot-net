# Namecheap-dot-net

A .NET wrapper for the [Namecheap API](http://www.namecheap.com/support/api/api.aspx).

## Sample Code

### Check the availability of several domain names.

```c#
	var api = new NameCheapApi("{username}", "{apiUser}", "{apiKey}", "{clientIp}", true);
	
	var domains = api.Domains.AreAvailable("google.com", "ANewDomainName");
	
	foreach (var domain in results)
		Console.WriteLine(domain.DomainName + ": " + domain.IsAvailable);	
```

### Set DNS host records for a given domain.

```c#
	var api = new NameCheapApi("{username}", "{apiUser}", "{apiKey}", "{clientIp}", true);
	 
	 api.Dns.SetHosts(new DnsHostsRequest()
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
---
title: Virtual Hosts
description: 'Allows to handle requests depending on the host specified by the client (to serve multiple domains using a single server).'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.VirtualHosting/" title="GenHTTP.Modules.VirtualHosting" icon="link" >}}
{{< /cards >}}

The virtual host handler can be used to deliver different content
depending on the host requested by the client. Note, that the
handler can be used anywhere in the handler chain, but usually it is
most benefitial as the main handler.

```csharp
var hosts = VirtualHosts.Create()
                        .Add("domain1.com", Layout.Create())
                        .Add("domain2.com", Layout.Create())
                        .Default(Layout.Create());
                        
await Host.Create()
          .Handler(hosts)
          .RunAsync();
```

The default route will be taken, if either the client did not 
send a host header, or the given host is not registered as a
virtual host. The default route can be omitted, resuling in
HTTP 404 error pages being returned by the server in such cases.

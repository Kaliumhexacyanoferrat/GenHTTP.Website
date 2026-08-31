---
title: Kestrel
weight: 2
description: 'Run GenHTTP applications on the Kestrel web server maintained by Microsoft.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Core.Kestrel/" title="GenHTTP.Core.Kestrel" icon="link" >}}
{{< /cards >}}

Kestrel is a web server developed by Microsoft and the engine that runs ASP.NET applications.
As it is maintained by Microsoft, this engine is a good choice when you have
high performance and security requirements.

```csharp
using GenHTTP.Engine.Kestrel;

await Host.Create()
          .Handler(...)
          .RunAsync();
```

If needed, you can pass custom hooks to the host builder to adjust
the underlying `WebApplication`:

```csharp 
using GenHTTP.Engine.Kestrel;

using Microsoft.AspNetCore.Builder;

var configHook = (WebApplicationBuilder b) => { 
    // adjust the builder here
};

var appHook = (WebApplication a) => { 
    // adjust your app here
};

await Host.Create(configHook, appHook)
          .Handler(...)
          .RunAsync();
```

In contrast to the internal engine, Kestrel supports HTTP/2 and HTTP/3 via SSL/TLS. While HTTP/2 is enabled
by default on such endpoints, you need to opt-in to HTTP/3, as the protocol is served via UDP/QUIC which probably
requires additional firewall rules on your system:

```csharp
.Bind(IPAddress.Any, 443, myCertificate, enableQuic: true)
```

There are some limitations that apply to this engine:

- Kestrel does not allow to read the request body twice (there is `request.EnableBuffering()` but this has not been implemented yet)
- Kestrel does not allow to read the size of the request body
- Kestrel does not allow to set a custom HTTP response status
- Websockets are currently not supported

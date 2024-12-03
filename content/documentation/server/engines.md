---
title: Engines
weight: 1
description: 'Describes the webserver engines available to run GenHTTP projects with (such as Kestrel).'
cascade:
  type: docs
---

GenHTTP is both a HTTP server implementation and a web service application framework. 
Depending on your requirements, the underlying HTTP engine can be replaced with another web server.

The acceptance tests of the project ensure that you can replace the engine without any further
adjustments to your application code - so basically all you do is changing the core nuget package.

## Internal

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Core/" title="GenHTTP.Core" icon="link" >}}
{{< /cards >}}

This is the default HTTP implementation provided by the GenHTTP framework. It 
relies only on code written in C# and tries to reduce dependencies to third-party
libraries and the environment it runs in as much as possible, therefore making it
a good choice for embedding a webservice into another application (e.g. WPF or Windows Forms)
or hosting a small service as a Docker container.

## Kestrel

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Core.Kestrel/" title="GenHTTP.Core.Kestrel" icon="link" >}}
{{< /cards >}}

Kestrel is a web server developed by Microsoft and the engine that runs ASP.NET applications.
As it is maintained by Microsoft, this engine is a good choice when you have
high performance and security requirements.

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

## Custom Engines

If you are interested in adding a new engine to the project, feel free
to get in touch via Discord. The basic requirements are:

- Your engine is written for the .NET platform
- The engine passes the acceptance tests of the project
- The change does not affect the stability of the CI pipeline of the project

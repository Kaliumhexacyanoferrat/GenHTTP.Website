---
title: Adapters
description: Use GenHTTP modules in other webserver frameworks such as ASP.NET.
weight: 6
cascade:
  type: docs
---

GenHTTP is both a web server and a web application development framework. Adapters
allow to re-use the functionality of the GenHTTP modules in other server frameworks
such as ASP.NET Core.

## ASP.NET Core

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Adapters.AspNetCore/" title="GenHTTP.Adapters.AspNetCore" icon="link" >}}
{{< /cards >}}

This adapter allows to plug in any GenHTTP handler into an existing ASP.NET Core application.
The following example will use the [listing](../../handlers/listing/) module to render a graphical
file listing when accessing http://localhost:5000/files/ in the browser.

```csharp
using GenHTTP.Adapters.AspNetCore;

using GenHTTP.Modules.DirectoryBrowsing;
using GenHTTP.Modules.IO;

using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder();

var app = builder.Build();

var listing = Listing.From(ResourceTree.FromDirectory("."))
                     .Defaults();

app.Map("/files", listing);

await app.RunAsync();
```

You can use this functionality with any of the handlers and concerns provided by GenHTTP.
The `Default()` method will add some comfort features such as automatic response compression, 
client caching policies, range support and a default error handler.

## Wired.IO

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Adapters.WiredIO/" title="GenHTTP.Adapters.WiredIO" icon="link" >}}
{{< /cards >}}

[Wired.IO](https://github.com/MDA2AV/Wired.IO) is a lightweight, embeddable and highly-customizable webserver framework for .NET, designed for high performance and minimal overhead. It supports HTTP/1.1, TLS, WebSockets, and seamless integration with `IServiceCollection` / `IServiceProvider`, making it ideal for embedding in desktop, console, or hybrid applications.

The [adapter](https://github.com/Kaliumhexacyanoferrat/wired-genhttp-adapter) allows to combine the flexibility and performance of Wired.IO with the high-level APIs provided by the GenHTTP framework.

```csharp
using GenHTTP.Adapters.WiredIO;

using GenHTTP.Modules.ApiBrowsing;
using GenHTTP.Modules.Functional;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.OpenApi;

using Wired.IO.App;

// GET http://localhost:5000/api/redoc/

var api = Inline.Create()
                .Get("hello", (string a) => $"Hello {a}!");

var layout = Layout.Create()
                   .Add(api)
                   .AddOpenApi()
                   .AddRedoc()
                   .Defaults(); // adds compression, eTag handling, ...

var builder = WiredApp.CreateExpressBuilder()
                      .Port(5000)
                      .MapGenHttp("/api/*", layout);

var app = builder.Build();

await app.RunAsync();
```

## Unhinged

{{< cards >}}
{{< card link="https://www.nuget.org/packages/Unhinged.GenHttp.Experimental/" title="Unhinged.GenHttp.Experimental" icon="link" >}}
{{< /cards >}}

[Unhinged](https://github.com/MDA2AV/unhinged) is an experimental, Linux‑only high-performance socket engine written in C#. It is built around Linux’s epoll interface (accept4, etc.) for ultra-fast, low-level network I/O.

The [adapter](https://github.com/MDA2AV/Unhinged-GenHttp-Adapter) allows to combine the raw performance provided by Unhinged with the high-level APIs provided by the GenHTTP framework.
Please note that both Unhinged and the adapter are in a very development stage and also that this sample will run on `linux-x64` only.

```csharp
using GenHTTP.Modules.ApiBrowsing;
using GenHTTP.Modules.Functional;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.OpenApi;

using Unhinged;
using Unhinged.GenHttp.Experimental;

// GET http://localhost:8080/api/redoc/

var api = Inline.Create()
                .Get("hello", (string a) => $"Hello {a}!");

var layout = Layout.Create()
                   .Add(api)
                   .AddOpenApi()
                   .AddRedoc()
                   .Defaults(); // adds compression, eTag handling, ...

var engine = UnhingedEngine.CreateBuilder()
                           .SetNWorkersSolver(() => Environment.ProcessorCount / 2) // Nº of worker threads
                           .SetBacklog(16384)
                           .SetMaxEventsPerWake(512)
                           .SetMaxNumberConnectionsPerWorker(512)
                           .SetPort(8080)
                           .SetSlabSizes(16 * 1024, 16 * 1024) // Pinned unmanaged memory slabs size per connection
                           .Map(layout) // Register your GenHTTP app
                           .Build();

engine.Run();
```
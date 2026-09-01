---
title: Adapters
description: Use GenHTTP modules in other webserver frameworks such as ASP.NET.
weight: 2
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
The following example will use the [listing](../../content/handlers/listing/) module to render a graphical
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
The `Defaults()` method will add some comfort features to the outermost handler: response
compression, client caching (ETags) and a default error handler are enabled by default; range
support is opt-in via `Defaults(rangeSupport: true)`.

To map a handler to every request instead of a specific path prefix, use `Run()` on the
`IApplicationBuilder` instead of `Map()`:

```csharp
app.Run(listing);
```

`Run()` also accepts an optional `IServer` instance, if you would like GenHTTP components to be
able to resolve the server they are running on (e.g. for `IServer.Properties`) even though ASP.NET
Core, not GenHTTP, is actually hosting the connection.

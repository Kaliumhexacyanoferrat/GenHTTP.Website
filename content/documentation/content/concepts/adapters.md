---
title: Adapters
description: Use GenHTTP modules in other webserver frameworks such as ASP.NET.
weight: 5
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

---
title: Single Page Applications (SPA)
description: Simple way to host applications written with JS frameworks such as Vue.js, Angular or React.
weight: 5
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.SinglePageApplications/" title="GenHTTP.Modules.SPAs" icon="link" >}}
{{< /cards >}}

This handler provides an easy way to serve a single page application (for example a 
Vue.js, React, or Angular app) to your clients.

{{< callout type="info" >}}
Apps can quickly be created by using a [project template](../../templates).
{{< /callout >}}

## Hosting a SPA

```csharp
var tree = ResourceTree.FromDirectory("/var/html/my-webapp");

var app = SinglePageApplication.From(tree);

 Host.Create()
     .Console()
     .Defaults()
     .Handler(app)
     .Run();
```

This example will automatically search for an index file (such as `index.html`) in
the specified directory and serve it to clients accessing http://localhost:8080/.

## Routing

If you would like to use path based routing in your application, the server needs to
serve the SPA index for every route. This can be achieved with the `ServerSideRouting()`
method on the SPA builder.

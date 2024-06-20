---
title: Single Page Applications (SPA)
cascade:
  type: docs
---

This handler provides an easy way to serve a single page application (for example a 
Vue.js, React, or Angular app) to your clients.

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

With this handler, a single page application can be hosted with just a few lines of code
using [Docker](/documentation/hosting/).

## Routing

If you would like to use path based routing in your application, the server needs to
serve the SPA index for every route. This can be achieved with the `ServerSideRouting()`
method on the SPA builder.

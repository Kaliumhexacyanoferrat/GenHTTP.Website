## Single Page Applications (SPA)

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
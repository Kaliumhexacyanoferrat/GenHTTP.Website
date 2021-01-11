## Static Websites

This handler provides an easy way to serve a static website such as a [Hugo](https://gohugo.io/)
application to your clients.

```csharp
var tree = ResourceTree.FromDirectory("/var/html/my-website");

var app = StaticWebsite.From(tree);

 Host.Create()
     .Console()
     .Defaults()
     .Handler(app)
     .Run();
```

This example will make the specified application available on http://localhost:8080/.
The handler will search for index files (such as `index.html`) and automatically
generate a sitemap and robots instruction file. If 
those files already exist in the given web root, they will be served directly instead.

With this handler, a static website can be hosted with just a few lines of code
using [Docker](/documentation/hosting/).

If you would like to combine a static website with dynamic content such as a webservice,
you can use the handler as the `Fallback` of a [Layout](./layouting):

```csharp
var api = Layout.Create()
                .AddService<...>("...");

var content = Layout.Create()
                    .Add("api", api)
                    .Fallback(StaticWebsite.From(...))

 Host.Create()
     .Console()
     .Defaults()
     .Handler(app)
     .Run();
```
## Minification

The minification module allows you to minify typical web resources
such as JS, CSS or HTML files before serving them to the client.

```csharp
using GenHTTP.Modules.Minification;

var app = Layout.Create()
                .Add(...)
                .Minification();
```

The minification will only be performed if the server does not
run in development mode.

As minification happens on the fly everytime a resource is requested
by the client, you might want to combine the minification with
pre-compression and a server side cache. See section
"Pre-compress content" on [this page](./server-caching).

## Customizing Minification

If you would like to customize the default configuration, you can create
a new instance with no plugins and add them manually.

The following example shows how to minify CSS only:

```csharp
var minification = Minify.Empty()
                         .AddCss();

var app = Layout.Create()
                .Add(...)
                .Add(minification);
```

## Custom Plugins

If you would like to add a custom minification (e.g. to minify pictures
by sending them to a web service such as TinyPNG) you can add a class
implementing `IMinificationPlugin` and add an instance of that class
via the `Add()` call of the minification builder. See the existing minifications
(such as `CssPlugin`) in the server code for reference.
---
title: Layouting
description: 'Easily break down your web application into logical sections of content.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Layouting/" title="GenHTTP.Modules.Layouting" icon="link" >}}
{{< /cards >}}

Layouts are a basic mechanism that allows you to assemble your web application from
different parts. They define which URLs are made available by the server and what
the URL structure of your app will look like.

The following example will expose a single file to your clients which will be available
via the URL [http://localhost:8080/hello.txt](http://localhost:8080/hello.txt). For any other URL, the server will generate
a `404 Not Found` response.

```csharp
var app = Layout.Create()
                .Add("hello.txt", Content.From(Resource.FromString("Hello World")));
                   
await Host.Create()
          .Handler(app)
          .RunAsync();
```

In this example, we create a segment named `hello.txt` and let it be handled by the `Content` handler.

## Sub Sections

Layouts can contain another layout for a specific path. This way your web application can be structured
as needed. The following example exposes [http://localhost:8080/static/hello.txt](http://localhost:8080/static/hello.txt) to your clients.

```csharp
var resources = Layout.Create()
                      .Add("hello.txt", Content.From(Resource.FromString("Hello World")));

var app = Layout.Create()
                .Add("static", resources);

await Host.Create()
          .Handler(app)
          .RunAsync();
```

This feature is typically used to describe your project topology on a high level and
to glue everything together. The following example will give you an idea on how this
could be used in a more complex app.

```csharp
var app = Layout.Create()
                .Add("api", ...)
                .Add("static", ...)
                .Add("admin-area", ...);

await Host.Create()
          .Handler(app)
          .RunAsync();
```

### Multiple Segments

To create a nested path structure (such as `/api/v1/`) you can nest layouts within layouts. 
To make this easier, there is an extension method that accepts an array of segments and internally creates additional layouts as needed:

```csharp
var api = Layout.Create()
                .AddService<...>("...");

var app = Layout.Create()
                .Add(["api", "v1"], api);

await Host.Create()
          .Handler(app)
          .RunAsync();
```

Alternatively, you can also write `.Add("/api/v1/", api)` which is less type safe yet easier to understand.

### Imperative Flow

Instead of creating and adding new layouts for each sub section yourself, you can also directly create a new layout
by calling the `AddSegment()` method on an existing builder. This flavor feels more imperative and might suit some project
setup procedures better than the functional one.

```csharp
var app = Layout.Create();

var api = app.AddSegment("api"); // or AddSegments([ "api", "v1" ])

api.AddService<SomeService>("some");
```

## Fallbacks

When adding handlers without a segment name, they will be called if no other registered handler was able
to generate a response. In the following example, [http://localhost:8080/file/](http://localhost:8080/file/)
will return `File Contents` whereas any other URL will return `Fallback Content`.

```csharp
var app = Layout.Create()
                .Add("file", Content.From(Resource.FromString("File Content")))
                .Add(Content.From(Resource.FromString("Fallback Content")));

await Host.Create()
          .Handler(app)
          .RunAsync();
```

If there are multiple routes set on a layout, the implementation will invoke them one-by-one until it 
retrieves a non-null response. In the following example, the server will render the "Hello World"
response only, if the request file does not exist in the given directory.

```csharp
var tree = ResourceTree.FromDirectory("...");

var files = Resources.From(tree);

var app = Layout.Create()
                .Add(files)
                .Add(Content.From(Resource.FromString("Hello World")));

await Host.Create()
          .Handler(app)
          .RunAsync();
```

## Index

While the `Add()` method matches all routes that start with the given name (e.g. `/hello.txt/appendix`
in the first example of this document), the `Index()` method only matches the current root. The following example
will render `Hello World` when [http://localhost:8080/](http://localhost:8080/) is requested, but returns
`404 Not Found` for any other URL.

```csharp
var app = Layout.Create()
                .Index(Content.From(Resource.FromString("Hello World")));
                   
await Host.Create()
          .Handler(app)
          .RunAsync();
```

## Extensions

Some modules extend the layout builder to reduce the boilerplate code required
to add handlers or concerns to your application.

For example the webservice module allows to directly add a webservice handler.

```csharp
var api = Layout.Create()
                .AddService<MyServiceClass>("service");
```

which is a shortcut for

```csharp
var service = ServiceResource.From<MyServiceClass>();

var api = Layout.Create()
                .Add("service", service);
```

As those methods are defined in extension methods provided by the source module,
this requires you to add an using directive, in this case `using GenHTTP.Modules.Webservices`.

## Non-Root Layouts

While it makes sense to start with a layout on root level to define the structure
of your application, it is just an ordinary handler and your server instance
just requires any handler to work with.

This means that you do not need to use a layout as a root handler and can add
layouts anywhere where handlers are supported.

The following example uses the virtual host handler to serve to different kind of
apps to your clients:

```csharp
var app1 = Layout.Create();

var app2 = Layout.Create();

var app = VirtualHosts.Create()
                      .Add("domain1.com", app1)
                      .Add("domain2.com", app2);
                      
await Host.Create()
          .Handler(app)
          .RunAsync();            
```

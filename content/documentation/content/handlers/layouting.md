---
title: Layouting
description: 'Easily break down your web application into logical sections of content.'
weight: 1
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
via the URL [http://localhost:8080/hello.txt](http://localhost:8080/hello.txt).

```csharp
var app = Layout.Create()
                .Add("hello.txt", Content.From(Resource.FromString("Hello World")));
                   
await Host.Create()
          .Handler(app)
          .RunAsync();
```

## Index

To define what is returned when the index of your application is called, you can
omit the path when adding handlers. The following example will render "Hello World"
when [http://localhost:8080/](http://localhost:8080/) is requested.

```csharp
var app = Layout.Create()
                .Add(Content.From(Resource.FromString("Hello World")));
                   
await Host.Create()
          .Handler(app)
          .RunAsync();
```

## Sub Sections

Layouts can contain another layout for a specific path or the index route. This
way your web application can be structured as needed. The following example
exposes [http://localhost:8080/static/hello.txt](http://localhost:8080/static/hello.txt)
to your clients.

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
could be used in a more complex web app.

```csharp
var app = Layout.Create()
                .Add("api", ...)
                .Add("static", ...)
                .Add("admin-area", ...);

await Host.Create()
          .Handler(app)
          .RunAsync();
```

## Fallbacks

If a handler cannot provide a response it will return `null` which causes the server
to render a `HTTP 404` error page. If there are multiple index routes set on a layout,
the implementation will invoke them one-by-one until it retrieves a non-null response. This
allows you to implement fallbacks. In the following example, the server will render the "Hello World"
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

---
title: Documentation
description: 'Simple tutorial to setup a web application using the GenHTTP framework.'
cascade:
  type: docs
---

With the GenHTTP framework, developers can quickly create new web services.
Everything else is provided by the server infrastructure as well as the excellent
ecosystem of .NET which easily allows to build, test, and run applications.

Similar to other frameworks like Nancy, embedio, NetCoreServer, or Watson, and in comparison to ASP.NET,
GenHTTP emphasizes a gentle learning curve to enable fast results. You should be able to setup a new web service
or website in a couple of minutes.

## Getting Started

To host a GenHTTP server instance in an existing or new .NET project, add a nuget reference to `GenHTTP.Full` to your
project and spin off a new host:

```csharp
using GenHTTP.Engine.Internal;

using GenHTTP.Modules.ApiBrowsing;
using GenHTTP.Modules.Functional;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.OpenApi;
using GenHTTP.Modules.Practices;

// use a handler of your choice (see the samples below)
var api = Layout.Create()
                .Add(Inline.Create().Get(() => "Hello World"))
                .AddOpenApi()
                .AddScalar();

var host = await Host.Create()
                     .Handler(api)
                     .Defaults()
                     .StartAsync(); // or .RunAsync() to block until the (console) application is shut down
```

Running this snippet will provide the following endpoints:

| Endpoint                           | Description                                                            |
|------------------------------------|------------------------------------------------------------------------|
| http://localhost:8080              | Serves the API, answering requests with a "Hello World" text response. |
| http://localhost:8080/openapi.json | Serves the automatically generated Open API specification of the API.  |
| http://localhost:8080/scalar/      | Servers a graphical viewer of the API, using Scalar.                   |

## Samples

The [playground](https://github.com/Kaliumhexacyanoferrat/GenHTTP/tree/main/Playground) project provides a quick starting point to view sample code and find more complex apps
built with GenHTTP.

## Next Steps

The example project above gives you a basic idea of how projects developed
with GenHTTP might look like. To create more complex web applications, 
follow the guides in the following sections:

{{< cards >}}

  {{< card link="./content/" title="Implement your service" >}}
  
  {{< card link="./testing/" title="Test your logic" >}}
  
  {{< card link="./server/" title="Run your app" >}}
  
  {{< card link="./hosting/" title="Deploy your app" >}}

{{< /cards >}}

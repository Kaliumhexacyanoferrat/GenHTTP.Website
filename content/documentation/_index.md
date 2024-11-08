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

This section shows how to create a new project from scratch using project templates and how to extend your existing application by embedding the GenHTTP engine.

{{< callout emoji="🌐" >}}
This is a brief overview to get you running. You might want to have a look
at the [tutorials](./tutorials/) for detailed step-by-step guides.
{{< /callout >}}

### New Project

Project templates can be used to create apps for typical use cases with little effort. After installing the [.NET SDK](https://dotnet.microsoft.com/en-us/download) and the templates via `dotnet new -i GenHTTP.Templates` in the terminal, the templates are available via the console or directly in Visual Studio:

![GenHTTP template projects in Visual Studio](/documentation/content/templates.png)

To create a project by using the terminal, create a new folder for your app and use one of the following commands:

| Template                      | Command                                     | Documentation                                                                   |
|-------------------------------|---------------------------------------------|---------------------------------------------------------------------------------|
| REST Webservice               | `dotnet new genhttp-webservice`             | [Webservices](./content/frameworks/webservices)                                 |
| REST Webservice (single file) | `dotnet new genhttp-webservice-minimal`     | [Functional Handlers](./content/frameworks/functional)                          |
| REST Webservice (controllers) | `dotnet new genhttp-webservice-controllers` | [Controllers](./content/frameworks/controllers)                                 |
| Websocket                     | `dotnet new genhttp-websocket`              | [Websockets](./content/frameworks/websockets)                                   |
| Website (Static HTML)         | `dotnet new genhttp-website-static`         | [Statics Websites](./content/frameworks/static-websites)                        |
| Single Page Application (SPA) | `dotnet new genhttp-spa`                    | [Single Page Applications (SPA)](./content/frameworks/single-page-applications) |

After the project has been created, you can run it via `dotnet run` and access the server via http://localhost:8080.

### Extending Existing Apps

If you would like to extend an existing .NET application, just add a nuget reference to the `GenHTTP.Core` nuget package. You can then spawn a new server instance with just a few lines of code:

```csharp
var content = Content.From(Resource.FromString("Hello World!"));

var server = Host.Create()
                 .Handler(content)
                 .Defaults()
                 .StartAsync(); // or .RunAsync() to block until the application is shut down
```

When you run this sample it can be accessed in the browser via http://localhost:8080. 

## Next Steps

The example project above gives you a basic idea on how projects developed
with the GenHTTP may look like. To create more complex web applications, 
follow the guides in the following sections:

{{< cards >}}

  {{< card link="./tutorials/" title="Create a new project" >}}

  {{< card link="./content/" title="Implement your service" >}}
  
  {{< card link="./testing/" title="Test your logic" >}}
  
  {{< card link="./server/" title="Run your app" >}}
  
  {{< card link="./hosting/" title="Deploy your app" >}}

{{< /cards >}}

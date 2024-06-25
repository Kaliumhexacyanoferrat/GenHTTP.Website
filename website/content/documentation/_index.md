---
title: Documentation
cascade:
  type: docs
---

With the GenHTTP framework, developers should be able to easily develop new web
services in a short time. Everything else is provided by the server infrastructure as well as the excellent
ecosystem of .NET which easily allows to build, test, and run applications.

Similar to other frameworks such as Nancy, embedio, NetCoreServer, or Watson, and in comparison to ASP.NET,
GenHTTP focuses on a low learning curve to quickly achieve results. You should be able to setup a new web service
or website in a couple of minutes.

## Getting Started

This section shows how to create a new project from scratch using project templates and how to extend your existing application by embedding the GenHTTP engine.

### New Project

Project templates can be used to create apps for typical use cases with little effort. After installing the [.NET SDK](https://dotnet.microsoft.com/en-us/download) and the templates via `dotnet new -i GenHTTP.Templates` in the terminal, the templates are available via the console or directly in Visual Studio:

![GenHTTP template projects in Visual Studio](/documentation/content/templates.png)

To create a project by using the terminal, create a new folder for your app and use one of the following commands:

| Template | Command | Documentation |
|---|---|---|
| REST Webservice | `dotnet new genhttp-webservice` | [Webservices](./content/webservices) |
| REST Webservice (single file) | `dotnet new genhttp-webservice-minimal` | [Functional Handlers](./content/functional) |
| Website (Static HTML) | `dotnet new genhttp-website-static`  | [Statics Websites](./content/static-websites) |
| Single Page Application (SPA) | `dotnet new genhttp-spa` | [Single Page Applications (SPA)](./content/single-page-applications) |

After the project has been created, you can run it via `dotnet run` and access the server via http://localhost:8080.

### Extending Existing Apps

If you would like to extend an existing .NET application, just add a nuget reference to the `GenHTTP.Core` nuget package. You can then spawn a new server instance with just a few lines of code:

```csharp
var content = Content.From(Resource.FromString("Hello World!"));

using var server = Host.Create()
                       .Handler(content)
                       .Defaults()
                       .Start(); // or .Run() to block until the application is shut down
```

When you run this sample it can be accessed in the browser via http://localhost:8080. 

## Next Steps

The example project above gives you a basic idea on how projects developed
with the GenHTTP may look like. To create more complex web applications
(such as [webservices](/documentation/content/frameworks/webservices/)), follow the guides in the following sections:

- [Providing Content](/documentation/content/)
- [Testing](/documentation/testing/)
- [Server Setup](/documentation/server/)
- [Hosting Apps](/documentation/hosting/)

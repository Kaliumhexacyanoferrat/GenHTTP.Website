## Why another Framework?

With [ASP.NET](https://dotnet.microsoft.com/apps/aspnet), there is already a
very good, scalable solution to develop and host web applications and services based
on the .NET framework.
But as of 2022, it seems that there are not that many other web development frameworks 
and servers left besides Microsoft's ASP.NET.
This project aims to bring more diversity to the beautiful ecosystem of .NET development,
also in the spirit of the open source community. If you are familiar with the ASP.NET
APIs, you may want to have a look at the [comparison sheet here](./asp-net-comparison/).

With the GenHTTP framework, developers should be able to easily develop new web
applications in a short time.
Everything else is provided by the server infrastructure as well as the excellent
ecosystem of .NET which easily allows to build, test, and run applications.

## Hello World!

> <span class="note">NOTE</span> If you directly want to start with a more complex app such as a website or webservice, have a look at our [project templates](/documentation/content/templates).

To create our first application using the GenHTTP framework from scratch, open a terminal
and enter the following command to create a new 
[.NET 7](https://dotnet.microsoft.com/download) application:

```bash
dotnet new console --framework net7.0 -o Project
```

This will create a new folder `Project`. Within this folder, run the following
command to add a nuget package reference to the [GenHTTP Core](https://www.nuget.org/packages/GenHTTP.Core/)
nuget package:

```bash
cd Project
dotnet add package GenHTTP.Core
```

You can then edit the generated Program.cs to setup a simple project using 
the GenHTTP server API:

```csharp
using GenHTTP.Engine;

using GenHTTP.Modules.IO;
using GenHTTP.Modules.Practices;

var content = Content.From(Resource.FromString("Hello World!"));

return Host.Create()
           .Console()
           .Defaults()
           .Handler(content)
           .Run();
```

To run our newly created project, simply execute:

```bash
dotnet run
```

This will host a new server instance on port 8080, allowing you to view the
newly created project in your browser by navigating to http://localhost:8080.

![Browser showing our example project](/images/hello_world.png)

## Next Steps

The example project above gives you a basic idea on how projects developed
with the GenHTTP may look like. To create more complex web applications
(such as [websites](/documentation/content/websites) or [webservices](/documentation/content/webservices)),
follow the guides in the following sections:

- [Providing Content](/documentation/content/)
- [Server Setup](/documentation/server/)
- [Hosting Apps](/documentation/hosting/)
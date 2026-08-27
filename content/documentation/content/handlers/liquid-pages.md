# Liquid Pages

[ Kinetq.LiquidPages.GenHTTP](https://www.nuget.org/packages/Kinetq.LiquidPages.GenHTTP/)
To serve Liquid templates through GenHTTP, install the `Kinetq.LiquidPages.GenHTTP` package alongside the core `Kinetq.LiquidPages` package. The handler wires the LiquidPages MVVM middleware directly into the GenHTTP request pipeline.

## Installation

```powershell
dotnet add package Kinetq.LiquidPages
dotnet add package Kinetq.LiquidPages.GenHTTP
```

## Setup

### 1. Register services

Use `AddLiquidPages` on your `IServiceCollection`, passing in the assemblies that contain your page models:

```csharp
services.AddLiquidPages(typeof(Program).Assembly);
```

### 2. Register page models at startup

Resolve `ILiquidStartup` and call `RegisterPageModels` before starting the host:

```csharp
var startup = serviceProvider.GetService<ILiquidStartup>();
await startup.RegisterPageModels();
```

### 3. Add the handler

Pass the `LiquidHandlerBuilder` to GenHTTP's `Host.Create()`:

```csharp
var middleware = serviceProvider.GetRequiredService<ILiquidResponseMiddleware>();

await Host.Create()
          .Handler(new LiquidHandlerBuilder(middleware))
          .Bind(IPAddress.Any, 8080)
          .RunAsync();
```

### Full `Program.cs` example

```csharp
using System.Net;
using GenHTTP.Engine.Internal;
using Kinetq.LiquidPages.Helpers;
using Kinetq.LiquidPages.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceProvider = BuildServices();
var startup = serviceProvider.GetRequiredService<ILiquidStartup>();
await startup.RegisterPageModels();

var middleware = serviceProvider.GetRequiredService<ILiquidResponseMiddleware>();

await Host.Create()
          .Handler(new LiquidHandlerBuilder(middleware))
          .Bind(IPAddress.Any, 8080)
          .RunAsync();

static IServiceProvider BuildServices()
{
    var services = new ServiceCollection();

    services.AddLogging(builder =>
    {
        builder.AddSimpleConsole(o =>
        {
            o.IncludeScopes = true;
            o.SingleLine = true;
            o.TimestampFormat = "hh:mm:ss ";
        }).SetMinimumLevel(LogLevel.Debug);
    });

    services.AddLiquidPages(typeof(Program).Assembly);

    return services.BuildServiceProvider();
}
```

## Creating pages

### Page model

Each page is backed by a C# class that extends `LiquidPageModel`. The `[LiquidPage]` attribute associates the class with a URL route (as a regex) and a Liquid template path:

```csharp
using Kinetq.LiquidPages.Models;
using Kinetq.LiquidPages.Pages;

[LiquidPage("^/$", "Pages/Home.liquid")]
public class HomeModel : LiquidPageModel
{
    public string Title { get; set; } = "Welcome to Home";

    public override Task OnGetAsync(LiquidRequestModel request)
    {
        // Populate model properties before the template renders
        return Task.CompletedTask;
    }
}
```

### Liquid template

All public properties on the page model are accessible in the template via the `view_model` object:

```liquid
{% capture page_content %}
    <h1>{{ view_model.title }}</h1>
{% endcapture %}

{% include 'Layouts/default.liquid' %}
```

## Error pages

Use `[LiquidErrorPage]` to map an HTTP status code to a dedicated template:

```csharp
using System.Net;
using Kinetq.LiquidPages.Models;
using Kinetq.LiquidPages.Pages;

[LiquidErrorPage(HttpStatusCode.NotFound, "ErrorPages/NotFound.liquid")]
public class NotFoundModel : LiquidPageModel
{
    public string Title { get; set; } = "Page Not Found";
    public string Message { get; set; } = "The page you are looking for was not found.";

    public override Task OnGetAsync(LiquidRequestModel request)
    {
        return Task.CompletedTask;
    }
}
```

## GenHTTP concerns

Because `LiquidHandlerBuilder` implements `IHandlerBuilder<LiquidHandlerBuilder>`, you can attach any standard GenHTTP concern (compression, client caching, CORS, etc.) before building:

```csharp
var middleware = serviceProvider.GetRequiredService<ILiquidResponseMiddleware>();

var handler = new LiquidHandlerBuilder(middleware)
    .Add(CompressedContent.Default())
    .Add(ClientCache.Policy().Duration(TimeSpan.FromMinutes(10)));

await Host.Create()
          .Handler(handler)
          .Bind(IPAddress.Any, 8080)
          .RunAsync();
```

## Project layout

A typical project using this handler looks like this:

```
MyApp/
├── Program.cs
├── Pages/
│   ├── Home.liquid          # Liquid template
│   └── Home.liquid.cs       # Page model code-behind
├── ErrorPages/
│   ├── NotFound.liquid
│   └── NotFound.liquid.cs
└── Layouts/
    └── default.liquid
```

> **Tip:** Install the [Kinetq.LiquidPages.Extension](https://marketplace.visualstudio.com/items?itemName=Kinetq.LiquidPagesExtension) from the Visual Studio Marketplace to get syntax highlighting, a Prettier-based formatter, and scaffolding commands that automatically nest `.liquid.cs` files under their template in Solution Explorer.

## Further reading

- [LiquidPages documentation](https://www.kinetq.com/docs/open-source-software/liquid-pages)
- [Sample project](https://github.com/kinetq/liquid-simple-server/tree/master/src/Kinetq.LiquidPages.GenHTTP.Sample)
- [Fluid templating engine](https://github.com/sebastienros/fluid)
- [Liquid template language](https://shopify.github.io/liquid/)

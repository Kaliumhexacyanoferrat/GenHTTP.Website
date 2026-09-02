---
title: Liquid Pages
description: 'Renders HTML pages from Liquid templates using a page model based approach.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/Kinetq.LiquidPages.GenHTTP/" title="Kinetq.LiquidPages.GenHTTP" icon="link" >}}
{{< card link="https://www.nuget.org/packages/Kinetq.LiquidPages/" title="Kinetq.LiquidPages" icon="link" >}}
{{< /cards >}}

{{< callout type="info" >}}
  This is a third-party integration maintained by [Kinetq](https://www.kinetq.com/), not part of the
  GenHTTP project. Questions and issues regarding the package are best directed at its maintainers.
{{< /callout >}}

Liquid Pages renders HTML from [Liquid](https://shopify.github.io/liquid/) templates, evaluated by the
[Fluid](https://github.com/sebastienros/fluid) engine. Each page is backed by a C# model class that
populates the values a template renders, so page logic and markup stay separated. The
`Kinetq.LiquidPages.GenHTTP` package provides a handler that runs this rendering inside the GenHTTP
request pipeline.

Unlike the built-in [Pages](../pages/) module, which renders individual Cottle templates on demand,
Liquid Pages resolves templates from page models that are discovered and mapped to routes at startup.
It relies on `Microsoft.Extensions.DependencyInjection`, so it is a good fit for applications that
already use a service container.

## Setup

Register the Liquid Pages services on your `IServiceCollection`, passing the assemblies that contain
your page models. During startup, `ILiquidStartup.RegisterPageModels` discovers those models and maps
them to their routes. The handler itself is created from the `ILiquidResponseMiddleware` resolved from
the container and passed to `Host.Create()`:

```csharp
using System.Net;

using GenHTTP.Engine.Internal;

using Kinetq.LiquidPages.Helpers;
using Kinetq.LiquidPages.Interfaces;

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddLiquidPages(typeof(Program).Assembly);

var provider = services.BuildServiceProvider();

var startup = provider.GetRequiredService<ILiquidStartup>();

await startup.RegisterPageModels();

var middleware = provider.GetRequiredService<ILiquidResponseMiddleware>();

await Host.Create()
          .Handler(new LiquidHandlerBuilder(middleware))
          .Bind(IPAddress.Any, 8080)
          .RunAsync();
```

## Pages

A page consists of a model class and a Liquid template. The model extends `LiquidPageModel` and carries
a `[LiquidPage]` attribute that associates it with a URL route (given as a regular expression) and the
path to its template. The `OnGetAsync` override runs before the template is rendered and is where the
model's properties are populated:

```csharp
using Kinetq.LiquidPages.Models;
using Kinetq.LiquidPages.Pages;

[LiquidPage("^/$", "Pages/Home.liquid")]
public class HomeModel : LiquidPageModel
{
    public string Title { get; set; } = "Welcome";

    public override Task OnGetAsync(LiquidRequestModel request)
    {
        // populate model properties before the template renders
        return Task.CompletedTask;
    }
}
```

All public properties of the model are exposed to the template through the `view_model` object:

```liquid
{% capture page_content %}
<h1>{{ view_model.title }}</h1>
{% endcapture %}

{% include 'Layouts/default.liquid' %}
```

## Error Pages

A model carrying the `[LiquidErrorPage]` attribute maps an HTTP status code to its own template. It is
rendered whenever the server produces the corresponding status:

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

## Concerns

`LiquidHandlerBuilder` implements `IHandlerBuilder<LiquidHandlerBuilder>`, so the standard GenHTTP
[concerns](../../concerns/) can be attached to it like to any other handler:

```csharp
using GenHTTP.Modules.ClientCaching;
using GenHTTP.Modules.Compression;

var handler = new LiquidHandlerBuilder(middleware)
    .Add(CompressedContent.Default())
    .Add(ClientCache.Policy().Duration(7));

await Host.Create()
          .Handler(handler)
          .Bind(IPAddress.Any, 8080)
          .RunAsync();
```

## Further Reading

- [Liquid Pages documentation](https://www.kinetq.com/docs/open-source-software/liquid-pages)
- [Sample project](https://github.com/kinetq/liquid-simple-server/tree/master/src/Kinetq.LiquidPages.GenHTTP.Sample)
- [Liquid template language](https://shopify.github.io/liquid/)

---
title: Localization
description: Automatic language negotiation to serve localized content. 
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.I18n/" title="GenHTTP.Modules.I18n" icon="link" >}}
{{< /cards >}}

The localization concern analyzes incoming requests for 
language preferences and allows the application logic to
serve content based on those.

## Basic Usage

By default, the concern will read the `Accept-Language` header
and set the `CultureInfo.CurrentUICulture`accordingly. 

```csharp
using System.Globalization;

using GenHTTP.Engine.Internal;

using GenHTTP.Modules.Functional;
using GenHTTP.Modules.I18n;
using GenHTTP.Modules.Practices;

var localization = Localization.Create();

var app = Inline.Create()
                .Get(() => CultureInfo.CurrentUICulture.ToString())
                .Add(localization);

await Host.Create()
          .Handler(app)
          .Defaults()
          .Development()
          .Console()
          .RunAsync();
```

Running and accessing this example app via http://localhost:8080 in your
browser will print the preferred locale of your client.

## Language Negotiation

Typically your application will not support _any_ language,
so the client and the server need to negotiate a language
that suits the client and the server can provide. Therefore,
the client will send a list of supported languages and their
ranking in the `Accept-Language` header, such as `de,de-DE;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6,de-CH;q=0.5`.

On the server side, you need to specify the cultures that
are supported by your application, either statically or
dynamically:

```csharp
var localization = Localization.Create();

// statically set supported cultures
CultureInfo[] supported = [CultureInfo.CreateSpecificCulture("de"), CultureInfo.CreateSpecificCulture("fr")];

localization.Supports(supported);

// dynamically evaluate support
localization.Supports(culture => culture.EnglishName.Contains("ish"));
```

The localization concern will then try to find the language
requested by the client which is supported by the server and has
the highest priority on the client-side. If there is no intersection
between the two, the logic will fallback to the default locale
that is used by the server environment. The default value can be overridden
as needed:

```csharp
var localization = Localization.Create()
                               .Default(CultureInfo.CreateSpecificCulture("pl-PL"));
```

## Parameter Source

As shown above, the server will analyze the `Accept-Language` header by
default to check the languages requested by the client. For API or web
applications you might want a different source instead:

```csharp
// read a query parameter, e.g. ?language=es
localization.FromQuery("language");

// read a cookie
localization.FromCookie("__my_app_language");

// read a custom header
localization.FromHeader("X-Custom-Language");

// read directly from the request, e.g. /de/...
localization.FromRequest(r => r.Target.Current?.Value);
```
If needed, you can specify multiple sources at the same time.

## Consuming the Language

As shown above, the server will set the `CurrentUICulture` of the current
thread. This behavior can be adjusted as needed:

```csharp
// set current culture instead
localization.Setter(currentCulture: true, currentUICulture: false);

// use a custom logic
localization.Setter((r, c) => r.GetUser<MyAppUser>()?.Language = c);
```

## Running in Docker

If you run your app in Docker (especially using an alpine-based images), 
setting the current culture in .NET will throw an exception. For this to work,
you will need to set the following settings.

In your `.csproj` file:

```xml
<InvariantGlobalization>false</InvariantGlobalization>
```

In your docker file:

```dockerfile
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
```

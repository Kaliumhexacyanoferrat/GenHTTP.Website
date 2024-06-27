---
title: Content
weight: 2
description: 'Tutorials to write web applications (such as webservices or websites) using the GenHTTP server framework.'
cascade:
  type: docs
---

This section describes the providers that you can use to implement your application, such
as webservices, redirects or static resources.

When running a GenHTTP server instance, you need to pass the root handler that will
be responsible to answer HTTP requests. You will typically start with a [layout](./handlers/layouting/) 
that allows to structure your web application and add some of the handlers below to 
achieve the required functionality to this layout.

The [template projects](./templates/) will already provide a basic project structure that
you can extend to your needs.

```csharp
var app = Layout.Create()
                .Add("resources", ...)
                .Add("api", ...);

Host.Create()
    .Handler(app)
    .Run();
```

This page lists all handlers that are provided by the framework. To implement new
functionality, you can also implement [custom handlers](./handlers/).

## Application Frameworks

{{< cards >}}

  {{< card link="./frameworks/webservices/" title="Webservices" >}}

  {{< card link="./frameworks/functional/" title="Functional Handlers" >}}

  {{< card link="./frameworks/controllers/" title="Controllers" >}}

  {{< card link="./frameworks/static-websites/" title="Static Websites" >}}

  {{< card link="./frameworks/single-page-applications/" title="Single Page Applications (SPA)" >}}

{{< /cards >}}

## Concerns

{{< cards >}}

  {{< card link="./concerns/authentication/" title="Authentication" >}}

  {{< card link="./concerns/client-caching-policy/" title="Client Caching (Policy)" >}}

  {{< card link="./concerns/client-caching-validation/" title="Client Caching (Validation)" >}}

  {{< card link="./concerns/compression/" title="Compression" >}}

  {{< card link="./concerns/cors/" title="CORS" >}}

  {{< card link="./concerns/error-handling/" title="Error Handling" >}}

  {{< card link="./concerns/server-caching/" title="Server Caching" >}}

  {{< card link="./concerns/range-support/" title="Range Support" >}}

  {{< card link="./concerns/defaults/" title="Defaults" >}}

{{< /cards >}}

## Providers

{{< cards >}}

  {{< card link="./handlers/layouting/" title="Layouting" >}}
  
  {{< card link="./handlers/static-content/" title="Static Content" >}}
  
  {{< card link="./handlers/downloads/" title="Downloads" >}}
  
  {{< card link="./handlers/redirects/" title="Redirects" >}}
  
  {{< card link="./handlers/listing/" title="Directory Browsing" >}}
  
  {{< card link="./handlers/reverse-proxy/" title="Reverse Proxies" >}}
  
  {{< card link="./handlers/virtual-hosts/" title="Virtual Hosts" >}}
  
  {{< card link="./handlers/load-balancer/" title="Load Balancer" >}}

{{< /cards >}}

## Concepts

{{< cards >}}

  {{< card link="./concepts/definitions/" title="Method Definitions" >}}

  {{< card link="./concepts/resources/" title="Resources" >}}

  {{< card link="./concepts/caches/" title="Caches" >}}

{{< /cards >}}



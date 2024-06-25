---
title: Content
description: 'Tutorials to write web applications (such as webservices or websites) using the GenHTTP server framework.'
cascade:
  type: docs
---

Starting a GenHTTP server instance will always require you to specify the handler
that is responsible to generate responses to client requests. Depending on the kind
of content you would like to serve (such as a webservice), there are various
handlers already available to be used.

```csharp
Host.Create()
    .Handler(...)
    .Run();
```

You will typically start with a [layout](./handlers/layouting/) that allows to structure
your web application and add the handlers needed to achieve the required functionality to this layout.

This page lists all handlers that are provided by the framework. To implement new
functionality, you can also implement [custom handlers](./handlers/).

To create a new project from scratch, it is recommended to use a [project template](./templates/).

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

## Misc

{{< cards >}}

  {{< card link="./concepts/definitions/" title="Method Definitions" >}}

  {{< card link="./concepts/resources/" title="Resources" >}}

  {{< card link="./concepts/caches/" title="Caches" >}}

{{< /cards >}}



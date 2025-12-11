---
title: Reverse Proxies
description: 'Server component to relay requests to an upstream server and return the result to requesting clients.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.ReverseProxy/" title="GenHTTP.Modules.ReverseProxy" icon="link" >}}
{{< /cards >}}

The reverse proxy content provider allows to embed content from another
web server into your application.

```csharp
var proxy = ReverseProxy.Create()
                        .Upstream("http://my-cdn:8080/");

await Host.Create()
          .Handler(proxy)
          .RunAsync();
```

When running this example, any request to http://localhost:8080 will be
proxied to http://my-cdn:8080/. Proxying works both for regular HTTP
requests and websocket connections.

If the upstream server is either not available or does not respond in time,
the provider will return an HTTP 502/504 error page instead.

## Adjustments

The module internally uses the `HttpClient` to perform HTTP requests. If needed,
you can pass actions to adjust the client as required.

```csharp
var proxy = ReverseProxy.Create()
                        .Upstream("http://my-cdn:8080/")
                        .AdjustHandler(h => h.Proxy = null)
                        .AdjustClient(c => c.MaxResponseContentBufferSize = 10000);
```

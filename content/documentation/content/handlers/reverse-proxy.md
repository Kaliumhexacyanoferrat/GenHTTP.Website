---
title: Reverse Proxies
description: 'Server component to relay requests to an upstream server and return the result to requesting clients.'
cascade:
  type: docs
---

The reverse proxy content provider allows to embed content from another
web server into your application. Content returned by the upstream
server will not be embedded into a templated page.

```csharp
Host.Create()
    .Handler(ReverseProxy.Create().Upstream("http://my-cdn:8080/"))
    .Run();
```

When running this example, any request to http://localhost:8080 will be
proxied to http://my-cdn:8080/.

If the upstream server is either not available or does not respond in time,
the provider will return a HTTP 502/504 error page instead.

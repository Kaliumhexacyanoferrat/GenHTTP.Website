## Reverse Proxies

The reverse proxy content provider allows to embed content from another
web server into your application. Content returned by the upstream
server will not be embedded into a templated page.

```csharp
layout.Add("downloads", ReverseProxy.Create().Upstream("http://my-cdn:8080/"));
```

If the upstream server is either not available or does not respond in time,
the provider will return a HTTP 502/504 error page instead.
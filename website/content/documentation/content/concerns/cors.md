---
title: CORS
cascade:
  type: docs
---

[Cross-Origin Resource Sharing](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS) allows to control
which resources browsers are allowed to access and how they are allowed to do so. For example, if you would like
to access a [webservice](./webservices) from a [website](./websites) hosted on another domain, the browser will
send the `Origin` header along with requests to the webservice and analyze the response to determine, whether the server
allows clients to access the resource from this origin.

To allow all clients to access a resource with no restrictions, you can add a `Permissive` policy to your handler:

```csharp
using GenHTTP.Modules.Webservices;
using GenHTTP.Modules.Security;

var api = Layout.Create()
                .AddService<Resource1>("res1")
                .AddService<Resource2>("res2")
                .Add(CorsPolicy.Permissive());
```

This will set the required headers for requests as well as preflight requests. To restrict the access
to a specific origin, create a `Restrictive()` policy and add the configuration for the desired origin:

```csharp
using GenHTTP.Modules.Webservices;
using GenHTTP.Modules.Security;

var policy = CorsPolicy.Restrictive()
                       .Add("https://mydomain.com", null, null, null, true);

var api = Layout.Create()
                .AddService<Resource1>("res1")
                .AddService<Resource2>("res2")
                .Add(policy);
```

This way, browsers will deny requests that do not originate from `https://mydomain.com`. For development
purposes, you may combine the snippets above along with the `#if DEBUG` directive. 

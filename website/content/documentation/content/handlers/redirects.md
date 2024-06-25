---
title: Redirects
description: 'Redirects requesting clients to another internal or external resource.'
cascade:
  type: docs
---

The redirect provider allows to inform the client that the location of the actual resource
is different than the requested URI.

```csharp
Host.Create()
    .Handler(Redirect.To("http://google.com"))
    .Run();
```

In this example, accessing http://localhost:8080 will redirect the client to the
Google search engine. By default, the HTTP status 301 (permament redirect) will be sent, which
can get customized to a temporary HTTP 307 redirect.

If you would like to redirect the client to a resource that is hosted by your
application, you can also pass a symbolic route such as `{sitemap}` (see [routing](./websites#routing)).

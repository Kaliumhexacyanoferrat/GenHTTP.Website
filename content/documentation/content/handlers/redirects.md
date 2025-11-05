---
title: Redirects
description: 'Redirects requesting clients to another internal or external resource.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Redirects/" title="GenHTTP.Modules.Redirects" icon="link" >}}
{{< /cards >}}

The redirect provider allows to inform the client that the location of the actual resource
is different than the requested URI.

```csharp
await Host.Create()
          .Handler(Redirect.To("http://google.com"))
          .RunAsync();
```

In this example, accessing http://localhost:8080 will redirect the client to the
Google search engine. By default, the HTTP status 301 (permanent redirect) will be sent, which
can get customized to a temporary HTTP 307 redirect.

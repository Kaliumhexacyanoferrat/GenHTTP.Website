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
Google search engine. `Redirect.To(location, temporary)` accepts the target as an absolute or
relative URI string and a `temporary` flag (`false` by default) as its second argument. The
status code sent depends on both this flag and the request method: `GET`/`HEAD` requests receive
a `301 Moved Permanently` or, if `temporary` is set, a `307 Temporary Redirect`; other methods
receive a `308 Permanent Redirect` or a `303 See Other` instead, so that a client does not
silently replay a non-idempotent request against the new location.

```csharp
Redirect.To("https://genhttp.org", temporary: true);
```

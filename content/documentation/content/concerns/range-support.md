---
title: Range Support
description: Enables partial responses if requested by the client, e.g. to resume downloads.
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.IO/" title="GenHTTP.Modules.IO" icon="link" >}}
{{< /cards >}}

Partial responses allow clients to fetch a specified portion of 
a requested resource which can be helpful for download managers
to pause and resume downloads. As this functionality is not 
needed by every web application, [range support](https://developer.mozilla.org/en-US/docs/Web/HTTP/Range_requests) is
disabled by default.

To enable this feature on server level, you may pass an
additional flag to the [default configuration](../defaults).

```csharp
await Host.Create()
          .Handler(...)
          .Defaults(rangeSupport: true)
          .RunAsync();
```

Please note that the implementation does not support multiple ranges 
to be requested (which would then result in a multipart response).

## Scope

As the range support is implemented as a [concern](../), you
may add this functionality to any handler as needed.

```csharp
using GenHTTP.Modules.IO;

var files = ResourceTree.FromDirectory("/var/www/downloads");

layout.Add("downloads", Resources.From(files).AddRangeSupport());
```

﻿---
title: Directory Browsing
description: 'Simple web application to list the contents of a directory via HTTP.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.DirectoryBrowsing/" title="GenHTTP.Modules.DirectoryBrowsing" icon="link" >}}
{{< /cards >}}

The directory listing provider serves a simple web UI allowing users to browse directories
and files read from a [resource tree](../../concepts/resources/) below the specified path.

```csharp
var tree = ResourceTree.FromDirectory("/var/www/documents/");

await Host.Create()
          .Handler(DirectoryListing.From(tree))
          .RunAsync();
```

In this example, the listing view will be available at http://localhost:8080/.

![Directory listing served by the GenHTTP server](listing.png)

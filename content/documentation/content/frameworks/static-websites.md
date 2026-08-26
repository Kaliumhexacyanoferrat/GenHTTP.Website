---
title: Static Websites
description: Simple way to host static websites with sitemap and robots.txt support
weight: 6
cascade:
  type: docs
---

This handler provides an easy way to serve a static website such as a [Hugo](https://gohugo.io/)
application to your clients.

## Creating a Static Website

The following example will host the specified application available on http://localhost:8080/.

```csharp
var tree = ResourceTree.FromDirectory("/var/html/my-website");

var app = StaticWebsite.From(tree);

await Host.Create()
          .Console()
          .Defaults()
          .Handler(app)
          .RunAsync();
```

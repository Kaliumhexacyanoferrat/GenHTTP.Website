---
title: Static Websites
weight: 4
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.StaticWebsites/" title="GenHTTP.Modules.StaticWebsites" icon="link" >}}
{{< /cards >}}

This handler provides an easy way to serve a static website such as a [Hugo](https://gohugo.io/)
application to your clients.

{{< callout type="info" >}}
Static websites can quickly be created by using a [project template](../../templates).
{{< /callout >}}

## Creating a Static Website

The following example will host the specified application available on http://localhost:8080/.

```csharp
var tree = ResourceTree.FromDirectory("/var/html/my-website");

var app = StaticWebsite.From(tree);

 Host.Create()
     .Console()
     .Defaults()
     .Handler(app)
     .Run();
```

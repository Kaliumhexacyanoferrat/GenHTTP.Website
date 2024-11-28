---
title: Resources
description: An abstraction layer over file system capabilities to allow binary content to be fetched from any data source
weight: 4
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.IO/" title="GenHTTP.Modules.IO" icon="link" >}}
{{< /cards >}}

Resources provide an unified way to load and access binary data used by handlers
to achieve their functionality. For example, the [Download](../../handlers/downloads/) handler
serves a single file on request - where the content of the file originates from
is not important for the handler to achieve it's functionality.

```csharp
var resource = Resource.FromFile("/var/www/downloads/myvideo.mp4"); // or FromString, FromAssembly, ...

var download = Download.From(resource);

await Host.Create()
          .Handler(download)
          .RunAsync();
```

By implementing the `IResource` interface, a custom data source can be used to
provide resources, for example a database or a cloud blob storage.

## Resource Trees

Similar to the resources, resource trees provide an abstraction for a directory
structure that can be consumed by handlers such as the [Directory Browsing](../../handlers/listing/)
or the [Single Page Application](../../frameworks/single-page-applications/). 

```csharp
var tree = ResourceTree.FromDirectory("/var/www/downloads/"); // or FromAssembly, ...

var listing = Listing.From(tree);

await Host.Create()
          .Handler(listing)
          .RunAsync();
```

Virtual trees allow to combine different sources of resources into an unified tree:

```csharp
var tree = VirtualTree.Create()
                      .Add("index.html", Resource.FromFile(...))
                      .Add("config.js", Resource.FromAssembly(...))
                      .Add("dist", ResourceTree.FromDirectory(...));

var app = SinglePageApplication.From(tree);
```

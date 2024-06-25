---
title: Resources
weight: 4
cascade:
  type: docs
---

Resources provide an unified way to load and access binary data used by handlers
to achieve their functionality. For example, the [Download](./downloads) handler
serves a single file on request - where the content of the file originates from
is not important for the handler to achieve it's functionality.

```csharp
var resource = Resource.FromFile("/var/www/downloads/myvideo.mp4"); // or FromString, FromAssembly, ...

var download = Download.From(resource);

Host.Create()
    .Handler(download)
    .Run();
```

By implementing the `IResource` interface, a custom data source can be used to
provide resources, for example a database or a cloud blob storage.

## Resource Trees

Similar to the resources, resource trees provide an abstraction for a directory
structure that can be consumed by handlers such as the [Directory Browsing](./listing)
or the [Single Page Application](./single-page-applications). 

```csharp
var tree = ResourceTree.FromDirectory("/var/www/downloads/"); // or FromAssembly, ...

var listing = Listing.From(tree);

Host.Create()
    .Handler(listing)
    .Run();
```

Virtual trees allow to combine different sources of resources into an unified tree:

```csharp
var tree = VirtualTree.Create()
                      .Add("index.html", Resource.FromFile(...))
                      .Add("config.js", Resource.FromAssembly(...))
                      .Add("dist", ResourceTree.FromDirectory(...));

var app = SinglePageApplication.From(tree);
```

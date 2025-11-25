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

GenHTTP provides a thin abstraction layer to control access of binary resources
(such as files or blobs). This allows to re-use file-dependent handlers, independent from
whether the data comes from the file system, a database or somewhere else.

## Resources

Resources provide a unified way to load and access binary data used by handlers
to achieve their functionality. For example, the [Download](../../handlers/downloads/) handler
serves a single file on request - where the content of the file originates from
is not important for the handler to achieve its functionality.

```csharp
var resource = Resource.FromFile("/var/www/downloads/myvideo.mp4"); // or FromString, FromAssembly, ...

var download = Download.From(resource);

await Host.Create()
          .Handler(download)
          .RunAsync();
```

By implementing the `IResource` interface, a custom data source can be used to
provide resources, for example a database or a cloud blob storage.

### Built-in Providers

The following resource implementations are provided by the `IO` module:

| Example                                | Description                                              |
|----------------------------------------|----------------------------------------------------------|
| `Resource.FromString("Hello World")`   | Creates a resource from a string constant.               |
| `Resource.FromFile("binary.blob")`     | Creates a resource from the given file.                  |
| `Resource.FromAssembly("binary.blob")` | Loads the resource from the currently executed assembly. |

### Change Tracking

For some use cases it might be important to determine, whether a given resource has changed
since it has been accessed the last time. For example, the page handler will
re-compile its template whenever the underlying resource has changed.

```csharp
var resource = Resource.FromFile("...")
                       .Build()
                       .Track();

await using var content = await resource.GetContentAsync();

var changed = await content.HasChanged();
```

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

### Virtual Trees

Virtual trees allow to combine different sources of resources into an unified tree:

```csharp
var tree = VirtualTree.Create()
                      .Add("index.html", Resource.FromFile(...))
                      .Add("config.js", Resource.FromAssembly(...))
                      .Add("dist", ResourceTree.FromDirectory(...));

var app = SinglePageApplication.From(tree);
```

### Resolving Files

When using resource trees within a handler, you might want to search for
files based on the remaining path of the request to be routed. The `Find()`
extension provided by the `IO` module will attempt to find the requested resource
or resource node from the current routing context and automatically advance the target.

```csharp
// either folder or file will be set
// if both are null, routing did not succeed
var (folder, file) = await Tree.Find(request.Target);
```

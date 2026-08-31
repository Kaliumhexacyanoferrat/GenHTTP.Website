---
title: Files
description: 'Serve files from the file system, an assembly or another resource source, either individually or as a whole directory.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Files/" title="GenHTTP.Modules.Files" icon="link" >}}
{{< /cards >}}

The Files module serves [resources](../../concepts/resources/) to clients, either one at a time
with a fixed path or as a whole browsable directory tree. The content type of a file is
automatically determined by its extension unless overridden.

## Serving a Single File

`Asset.From(...)` accepts a resource, a `FileInfo` or a file path directly and serves it under
the path it is mounted at:

```csharp
var layout = Layout.Create()
                   .Add("agb.pdf", Asset.From("/var/www/documents/agb.pdf"));
```

In this example, the file would be available at http://localhost:8080/agb.pdf. To force a
specific content type instead of guessing it from the extension, use `.Type(...)`:

```csharp
Asset.From("./data.bin").Type(ContentType.ApplicationOctetStream);
```

### Downloads

To have the browser download the file instead of rendering it inline, call `.AsDownload(...)`,
optionally passing the file name to send to the client (falling back to the resource's own name
if omitted):

```csharp
var layout = Layout.Create()
                   .Add("agb.pdf", Asset.From("/var/www/documents/agb.pdf").AsDownload());
```

## Serving a Directory

`Assets.From(...)` serves a whole [resource tree](../../concepts/resources/#resource-trees) or
directory, resolving the requested path against it:

```csharp
var layout = Layout.Create();

// serve all embedded resources in the "Resources" sub folder of your project
var tree = ResourceTree.FromAssembly("Resources");

layout.Add("res", Assets.From(tree));

// or, directly from a folder on disk
layout.Add("res", Assets.From("./Resources"));

await Host.Create()
          .Handler(layout)
          .RunAsync();
```

For example, a stylesheet named `main.css` in the `styles` subfolder would be made available at
http://localhost:8080/res/styles/main.css.

## Precompressed Files

If your build already produces precompressed variants of your static assets (e.g. `main.css.br`
next to `main.css`), `AllowPrecompressed(...)` lets the handler serve those directly instead of
compressing the response on every request. The algorithms passed in are matched against the
client's `Accept-Encoding` header, tried in priority order, and looked up as
`<original path><separator><algorithm name>` (`.` by default):

```csharp
using GenHTTP.Modules.Compression.Algorithms;

var assets = Assets.From("./dist")
                   .AllowPrecompressed(new BrotliAlgorithm());
```

With this configuration, a request for `main.css` with `Accept-Encoding: br` will be answered
with the contents of `main.css.br` (falling back to `main.css` if no precompressed variant is
found or accepted). Internally this reuses the [routing target's](../../concepts/routing/#suffix-routing)
`CopyAndAppend()` to look up the suffixed path without disturbing the original routing state.

## Ioxide Engine

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.IoxideFiles/" title="GenHTTP.Modules.IoxideFiles" icon="link" >}}
{{< /cards >}}

When running on the [Ioxide engine](../../../server/engines/ioxide/), the `IoxideFiles` module
provides a specialized static file handler built directly on top of the engine's native I/O
layer: responses are baked ahead of time and revalidated via `statx` instead of being assembled
per request, and precompressed `.br`/`.gz` variants are negotiated the same way as above. It is
mounted the same way as `Assets`:

```csharp
var layout = Layout.Create()
                   .Add("static", IoxideFiles.From("./dist"));
```

As it depends on the Ioxide engine's native bindings, this module targets `net11.0` only.

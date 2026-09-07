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
compressing the response on every request:

```csharp
using GenHTTP.Modules.Compression.Algorithms;

var assets = Assets.From("./dist")
                   .AllowPrecompressed(new BrotliAlgorithm());
```

With this configuration, a request for `main.css` with `Accept-Encoding: br` will be answered
with the contents of `main.css.br` (falling back to `main.css` if no precompressed variant is
found or accepted).

## Ioxide Engine

When the directory based `Assets.From(...)` runs on the [Ioxide engine](../../../server/engines/ioxide/),
it automatically serves files through a handler built on the engine's native I/O layer: responses are
baked ahead of time and revalidated via `statx` instead of being assembled per request, while
precompressed variants are negotiated exactly as described above. No separate package or registration
is required - the same handler transparently falls back to the portable implementation on every other
engine.

```csharp
var layout = Layout.Create()
                   .Add("static", Assets.From("./dist"));
```

As the native layer caches file descriptors rather than touching the disk on every request, the baked
responses are re-scanned for external changes at most once per `RefreshInterval` (250 ms by default).
Lowering it picks up changes faster at the cost of more frequent stat calls; the setting has no effect
on other engines.

```csharp
var assets = Assets.From("./dist")
                   .RefreshInterval(TimeSpan.FromSeconds(1));
```

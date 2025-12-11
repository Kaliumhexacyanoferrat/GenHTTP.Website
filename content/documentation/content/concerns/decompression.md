---
title: Decompression
description: Automatically decompress the content of incoming requests.
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Compression/" title="GenHTTP.Modules.Compression" icon="link" >}}
{{< /cards >}}

This concern will analyze incoming requests for compressed content
and will automatically decompress the body using a suitable decompression
algorithm.

In contrast to automatic compression of responses, this concern is not enabled by
default and needs to be activated on the host:

```csharp
await Host.Create()
          .Handler(...)
          .Defaults(decompression: true)
          .RunAsync();
```

## Custom Algorithms

The concern analyzes the `Content-Encoding` header of incoming requests and
supports automatic decompression of `gzip`, `br` and `zstd` compressed 
request bodies.

If you would like to add support for an additional algorithm, you need to implement
and supply a `ICompressionAlgorithm` instance. The following example shows how to
add `deflate` support to your server:

```csharp
using System.IO.Compression;

using GenHTTP.Api.Content.IO;
using GenHTTP.Api.Infrastructure;
using GenHTTP.Api.Protocol;

using GenHTTP.Engine.Internal;

using GenHTTP.Modules.Compression;
using GenHTTP.Modules.Compression.Providers;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Practices;

var decompression = DecompressedContent.Default()
                                       .Add(new DeflateAlgorithm());

var app = Layout.Create()
                .Add(decompression);

await Host.Create()
          .Handler(app)
          .Defaults()
          .Development()
          .Console()
          .RunAsync();

public class DeflateAlgorithm : ICompressionAlgorithm
{

    public string Name => "deflate";

    public Priority Priority => Priority.Low;

    public IResponseContent Compress(IResponseContent content, CompressionLevel level)
    {
        return new CompressedResponseContent(content, target => new DeflateStream(target, level, false));
    }

    public Stream Decompress(Stream content)
    {
        return new DeflateStream(content, CompressionMode.Decompress, leaveOpen: true);
    }

}
```
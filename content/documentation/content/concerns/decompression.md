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
supports automatic decompression of `gzip`, `br` and `zstd` (.NET 11 only) compressed request bodies.

If you would like to add support for an additional algorithm, you need to implement
and supply a `ICompressionAlgorithm` instance - the same interface used to
[compress responses](../compression/#custom-algorithms), but only its `Decompress(Stream)` member
is relevant here. The following example shows how to add `deflate` support to your server:

```csharp
using System.IO.Compression;

using GenHTTP.Api.Content.IO;
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
          .RunAsync();

public class DeflateAlgorithm : ICompressionAlgorithm
{
    private static readonly AlgorithmName AlgorithmName = new("deflate");

    public AlgorithmName Name => AlgorithmName;

    public Priority Priority => Priority.Low;

    public IResponseContent Compress(IResponseContent content, CompressionLevel level)
    {
        throw new NotSupportedException("This algorithm is only used for decompression");
    }

    public Stream Decompress(Stream content)
    {
        return new DeflateStream(content, CompressionMode.Decompress, leaveOpen: true);
    }

}
```

Once a matching algorithm is found for the request's `Content-Encoding`, the concern calls
`request.WrapBody(...)` to transparently replace the request body with a `DecompressedBody`
that decompresses on read - handlers downstream keep reading `request.GetBody()` as usual and
never see the compressed bytes. `WrapBody` on `IRequest` is the same extension point other
body-rewriting concerns can build on; only one wrapper can be active per request.

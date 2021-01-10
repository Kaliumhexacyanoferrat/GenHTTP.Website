## Compression

The compression concern compresses content sent to the clients, if applicable. By default,
[gzip](https://www.gzip.org/) and [Brotli](https://github.com/google/brotli) are supported.

```csharp
var content = Layout.Create()
                    .Add(CompressedContent.Default());
```

The `Defaults()` method of the [Practices](https://www.nuget.org/packages/GenHTTP.Modules.Practices/)
module will automatically configure compression on server level:

```csharp
Host.Create()
    .Handler(...)
    .Defaults()
    .Run();
```

## Custom Algorithms

To add a custom compression algorithm to the server, you can implement the
[ICompressionAlgorithm](https://github.com/Kaliumhexacyanoferrat/GenHTTP/blob/master/API/Infrastructure/ICompressionAlgorithm.cs)
interface and register the implementing class with your server builder. For example,
the following implementation will add support for the `deflate` algorithm, which 
is not provided by the server out of the box:

```csharp
public class DeflateCompression : ICompressionAlgorithm
{

    public string Name => "deflate";

    public Priority Priority => Priority.Low;

    public IResponseContent Compress(IResponseContent content)
    {
        return new CompressedResponseContent(content, (target) => new DeflateStream(target, CompressionLevel.Fastest, false));
    }

}
                        
// registration
var server = Server.Create()
                   .Handler(...)
                   .Defaults(compression: false)
                   .Compression(CompressedContent.Default().Add(new DeflateCompression()));
```
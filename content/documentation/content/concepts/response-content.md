---
title: Response Content
description: Describes the default I/O capabilities when working with response content in GenHTTP.
weight: 4
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.IO/" title="GenHTTP.Modules.IO" icon="link" >}}
{{< /cards >}}

When sending content in response to a client request, `IResponseBuilder` has a single entry point for it:
`Content(IResponseContent? content)`. Everything about the content itself - its `Length`, `Type` and
encoding - is a property of the `IResponseContent` implementation being passed in, not something configured
separately on the builder. `Length` is the number of bytes the content will produce; if it is not known,
chunked transfer encoding is applied.

The API provides a minimal protocol to allow the server engines to accept and consume the content to be sent
to the client. It does (and should not) know about files, streams, assemblies and so on.

To bridge the gap between the API and the real world, there is the I/O module which provides typically
used types of content as well as extension methods to make it easier to work with content. Most of them
accept an optional `ContentType?` parameter, so you rarely need to construct an `IResponseContent`
implementation yourself just to set a type:

```csharp
using GenHTTP.Api.Protocol;

using GenHTTP.Modules.IO;

request.Respond()
       .Content("Hello World", ContentType.TextPlain);
```

## Content Type Guessing

When working with files that are not necessarily under your control, you might need to dynamically
determine the content type of a given file. For this, you can use the `GuessContentType()` extension
provided by the I/O module, available both on `string` file names and on [resources](../resources/)
(where it additionally checks the resource's own declared content type before falling back to guessing
from its name):

```csharp
using GenHTTP.Modules.IO;

var type = "style.css".GuessContentType() ?? ContentType.ApplicationOctetStream;

var resourceType = resource.GuessContentType(); // falls back to ApplicationForceDownload
```

## Content Implementations

The I/O module adds `StringContent`, `StreamContent` and `ResourceContent` which can be used
to send corresponding content to the client. Each takes the `ContentType` to use as a constructor
argument. The following example will show you how to send strings, streams or [resources](../resources/)
as a response:

```csharp
using GenHTTP.Api.Protocol;

using GenHTTP.Engine.Internal;

using GenHTTP.Modules.IO;
using GenHTTP.Modules.IO.Streaming;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Practices;
using GenHTTP.Modules.Webservices;

using StreamContent = GenHTTP.Modules.IO.Streaming.StreamContent;
using StringContent = GenHTTP.Modules.IO.Strings.StringContent;

var app = Layout.Create()
                .AddService<ContentExamples>("content");

await Host.Create()
          .Handler(app)
          .Defaults()
          .RunAsync();

class ContentExamples
{

    [ResourceMethod("get-string")]
    public IResponseBuilder GetString(IRequest request)
    {
        return request.Respond()
                      .Content(new StringContent("This is a string", ContentType.TextPlain));
    }

    [ResourceMethod("get-resource")]
    public IResponseBuilder GetResource(IRequest request)
    {
        var resource = Resource.FromString("This is a string") // or from any other source
                               .Build();

        return request.Respond()
                      .Content(new ResourceContent(resource, ContentType.TextPlain));
    }

    [ResourceMethod("get-stream")]
    public IResponseBuilder GetStream(IRequest request)
    {
        var stream = new MemoryStream("This is a string"u8.ToArray());

        return request.Respond()
                      .Content(new StreamContent(stream, ContentType.TextPlain, (ulong)stream.Length, null, stream.CalculateChecksumAsync));
    }

}
```

To simplify their usage, the module also adds extensions directly on `IResponseBuilder`, which
are recommended to be used if possible:

```csharp
using GenHTTP.Api.Protocol;

using GenHTTP.Engine.Internal;

using GenHTTP.Modules.IO;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Practices;
using GenHTTP.Modules.Webservices;

var app = Layout.Create()
                .AddService<ContentExamples>("content");

await Host.Create()
          .Handler(app)
          .Defaults()
          .RunAsync();

class ContentExamples
{

    [ResourceMethod("get-string")]
    public IResponseBuilder GetString(IRequest request)
    {
        return request.Respond()
                      .Content("This is a string");
    }

    [ResourceMethod("get-resource")]
    public IResponseBuilder GetResource(IRequest request)
    {
        var resource = Resource.FromString("This is a string") // or from any other source
                               .Build();

        return request.Respond()
                      .Content(resource);
    }

    [ResourceMethod("get-stream")]
    public IResponseBuilder GetStream(IRequest request)
    {
        var stream = new MemoryStream("This is a string"u8.ToArray());

        return request.Respond()
                      .Content(stream, ContentType.TextPlain, (ulong)stream.Length, checksumProvider: stream.CalculateChecksumAsync);
    }

}
```

## Custom Content Implementations

The following code shows an example on how we can implement `IResponseContent`
to efficiently serve the data stored in an entity record via `Content()`. Besides `Length`, an
implementation needs to expose its own `Type` and `Encoding` and write itself to the `IResponseSink`
passed to `WriteAsync`:

```csharp
public class Attachment
{

    public int Id { get; set; }

    public long Size { get; set; }

    public DateTime Modified { get; set; }

    public ReadOnlyMemory<byte> Data { get; set; }

}

public class AttachmentContent(Attachment attachment) : IResponseContent
{

    public ulong? Length => (ulong)attachment.Size;

    public ContentType? Type => ContentType.ApplicationForceDownload;

    public ReadOnlyMemory<byte>? Encoding => null;

    public ValueTask<ulong?> CalculateChecksumAsync() => new((ulong)attachment.Modified.Ticks);

    public ValueTask WriteAsync(IResponseSink sink)
    {
        sink.Writer.Write(attachment.Data.Span);
        return ValueTask.CompletedTask;
    }

}

public class AttachmentContentHandler : IHandler
{

    public ValueTask PrepareAsync(IServer server) => ValueTask.CompletedTask;

    public ValueTask<IResponse?> HandleAsync(IRequest request)
    {
        var id = request.Header.Query.GetEntry("id");

        if (id != null)
        {
            // load the entity from some DB
            var entity = ...

            return request.Respond()
                          .Content(new AttachmentContent(entity))
                          .Build();
        }

        return new();
    }
    
}

await Host.Create()
          .Handler(new AttachmentContentHandler())
          .Defaults()
          .RunAsync();
```

While this is more complex than simply returning a `Stream` from a web service,
it is way more efficient for caching as we can use the modification date of the entity
to check for changes.

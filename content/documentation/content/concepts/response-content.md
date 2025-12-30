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

When sending content in response to a client request, there are three concerns that need to be addressed
via the `IResponseBuilder`.

| Method      | Description                                                                                                                                |
|-------------|--------------------------------------------------------------------------------------------------------------------------------------------|
| `Content()` | The actual content to be sent to the client, an `IResponseContent` implementation.                                                         |
| `Type()`    | The type of the content so the client can interpret this information. Set via a `FlexibleContentType`.                                     |
| `Length()`  | The number of bytes that the content will produce. Required by the HTTP protocol. If not known, chunked transfer encoding will be applied. |

The API provides a minimal protocol to allow the server engines to accept and consume the content to be sent
to the client. It does (and should not) know about files, streams, assemblies and so on.

To bridge the gap between the API and the real world, there is the I/O modules which provides typically
used types of content as well as extension methods make it easier to work with content.

## Content Type Simplifications

The `IResponseBuilder` API requires you to set a `FlexibleContentType`, which can transport both
types unknown to the server and the additional `charset` information. In many cases, you probably
just want to set a known type. Therefore, the I/O module adds the following extensions:

```csharp
using GenHTTP.Api.Protocol;

using GenHTTP.Modules.IO;

request.Respond()
       .Type(ContentType.TextHtml) // by known type
       .Type("text/html"); // or by mime type
```

## Content Type Guessing

When working with files that are not necessarily under your control, you might need to dynamically
determine the content type of a given file. For this, you can use the `GuessContentType()` extension
provided by the I/O module:

```csharp
using GenHTTP.Modules.IO;

var type = "style.css".GuessContentType() ?? ContentType.ApplicationOctetStream;
```

## Content Implementations

The I/O module adds `StringContent`, `StreamContent` and `ResourceContent` which can be used
to send corresponding content to the client. The following example will show you how to send
strings, streams or [resources](../resources/) as a response:

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
          .Console()
          .RunAsync();

class ContentExamples
{

    [ResourceMethod("get-string")]
    public IResponseBuilder GetString(IRequest request)
    {
        return request.Respond()
                      .Content(new StringContent("This is a string"))
                      .Type(FlexibleContentType.Get(ContentType.TextPlain));
    }

    [ResourceMethod("get-resource")]
    public IResponseBuilder GetResource(IRequest request)
    {
        var resource = Resource.FromString("This is a string") // or from any other source
                               .Build();

        return request.Respond()
                      .Content(new ResourceContent(resource))
                      .Type(FlexibleContentType.Get(ContentType.TextPlain));
    }

    [ResourceMethod("get-stream")]
    public IResponseBuilder GetStream(IRequest request)
    {
        var stream = new MemoryStream("This is a string"u8.ToArray());

        return request.Respond()
                      .Content(new StreamContent(stream, (ulong)stream.Length, stream.CalculateChecksumAsync))
                      .Type(FlexibleContentType.Get(ContentType.TextPlain));
    }

}
```

To simplify their usage, the module also adds some extensions to the `IResponseBuilder`, which
are recommended to be used if possible:

```csharp
using GenHTTP.Api.Protocol;

using GenHTTP.Engine.Internal;

using GenHTTP.Modules.IO;
using GenHTTP.Modules.IO.Streaming;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Practices;
using GenHTTP.Modules.Webservices;

var app = Layout.Create()
                .AddService<ContentExamples>("content");

await Host.Create()
          .Handler(app)
          .Defaults()
          .Console()
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
                      .Content(stream, (ulong)stream.Length, stream.CalculateChecksumAsync)
                      .Type(FlexibleContentType.Get(ContentType.TextPlain));
    }

}
```

## Custom Content Implementations

The following code shows an example on how we can implement `IResponseContent`
to efficiently serve the data stored in an entity record via `Content()`.

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

    public ValueTask<ulong?> CalculateChecksumAsync() => new((ulong)attachment.Modified.Ticks);

    public ValueTask WriteAsync(Stream target, uint bufferSize) => target.WriteAsync(attachment.Data);

}

public class AttachmentContentHandler : IHandler
{

    public ValueTask PrepareAsync() => ValueTask.CompletedTask;

    public ValueTask<IResponse?> HandleAsync(IRequest request)
    {
        if (request.Query.TryGetValue("id", out var id))
        {
            // load the entity from some DB
            var entity = ...

            return request.Respond()
                          .Content(new AttachmentContent(entity))
                          .Type(ContentType.ApplicationForceDownload)
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

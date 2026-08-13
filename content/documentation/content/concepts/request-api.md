---
title: Request API
description: Describes the core request/response model of GenHTTP.
weight: 1
cascade:
  type: docs
---

## Request API

To handle incoming HTTP requests, GenHTTP uses the `IRequest` abstraction, which - besides
granting access to infrastructure components such as the `IServer`, `IEndPoint` or `IClientConnection`,
is intended to represent the data on the wire as it was received by the connected client.

The core entry points to handle requests are [handlers](../../handlers/) and [concerns](../../concerns/).

```csharp
public interface IRequest
{

    IRequestHeader Header { get;}

    IRequestBody? GetBody(HeaderAccess headerAccess = HeaderAccess.Retain);

}
```

### Memory Views

As the underlying data is just bytes (or better `ReadOnlyMemory<byte>`), the API uses the
`MemoryView` code generator to create read-only structs that provide type safety and convenience methods 
on HTTP protocol elements. This is the key element that allows GenHTTP to be allocation-free in its core middleware
(and therefore being fast) while providing a high level of convenience to the API users.

```csharp
var isHead = request.Header.Method == RequestMethod.Head;
```

Memory views can either be constructed from memory (which happens when the request is constructed by the framework)
or from strings (which is the preferred way during compile/initialization time, e.g. as a static readonly field in handler code).

### Header Retention

## Response Generation
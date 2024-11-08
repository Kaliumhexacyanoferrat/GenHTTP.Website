---
title: Handlers
description: Low level handling of requests and generation of HTTP web server responses.
weight: 3
cascade:
  type: docs
---

Handlers are responsible for analyzing HTTP requests and to serve responses to the connected client. All features
available in the SDK are either provided by handlers or [concerns](../concerns).

If you would like to add an additional feature that cannot be achieved using the existing
functionality to the SDK or your own web application, you may implement
a custom handler instance. The following example will generate a simple text response:

```csharp
using GenHTTP.Api.Content;
using GenHTTP.Api.Protocol;

using GenHTTP.Engine;

using GenHTTP.Modules.IO;

public class CustomHandler : IHandler
{ 

    public ValueTask PrepareAsync() 
    {
        // perform CPU or I/O heavy work to initialize this
        // handler and it's children
        return new ValueTask();
    }
    
    public ValueTask<IResponse?> HandleAsync(IRequest request)
    {
        var response = request.Respond()
                              .Content("Hello World")
                              .Type(new FlexibleContentType(ContentType.TextPlain))
                              .Build();

        return new ValueTask<IResponse?>(response);
    }

}

public class CustomHandlerBuilder : IHandlerBuilder<CustomHandlerBuilder>
{
    private readonly List<IConcernBuilder> _Concerns = [];

    public CustomHandlerBuilder Add(IConcernBuilder concern)
    {
        _Concerns.Add(concern);
        return this;
    }

    public IHandler Build()
    {
        return Concerns.Chain(_Concerns, new CustomHandler());
    }

}

await Host.Create()
          .Handler(new CustomHandlerBuilder())
          .RunAsync();
```

The usage of the builder pattern is not required here but you will notice that all handlers
provided by the framework use this scheme (which basically allows to add concerns everywhere).

As handlers are invoked for every request handled by the server, it is usually worth it to
optimize them for performance. For example, as the content served by our `CustomHandler` does
not change depending on the request, the string resource instance could be cached by a field or property
set in the constructor.

## Handlers

Handlers are responsible for analyzing HTTP requests and to serve responses to the connected client. All features
available in the SDK are either provided by handlers or [concerns](./concerns).

If you would like to add an additional feature to the SDK or your own web application, you may implement
a custom handler instance. The following example will generate a simple text response:

```csharp
public class CustomHandler : IHandler
{ 

    public IHandler Parent { get; }

    public CustomHandler(IHandler parent)
    {
        Parent = parent;
    }

    public IResponse? Handle(IRequest request)
    {
        return request.Respond()
                      .Content(new StringContent("Hello World"))
                      .Type(new FlexibleContentType(ContentType.TextPlain))
                      .Build();
    }

    public IEnumerable<ContentElement> GetContent(IRequest request) => Enumerable.Empty<ContentElement>();

}

public class CustomHandlerBuilder : IHandlerBuilder<CustomHandlerBuilder>
{
    private readonly List<IConcernBuilder> _Concerns = new List<IConcernBuilder>();

    public CustomHandlerBuilder Add(IConcernBuilder concern)
    {
        _Concerns.Add(concern);
        return this;
    }

    public IHandler Build(IHandler parent)
    {
        return Concerns.Chain(parent, _Concerns, (p) => new CustomHandler(p));
    }

}

Host.Create()
    .Handler(new CustomHandlerBuilder())
    .Run();
```

As handlers are invoked for every request handled by the server, it is usually worth it to
optimize them for performance. For example, as the content served by our `CustomHandler` does 
not change depending on the request, the `StringContent` instance could be cached by a field or property
set in the constructor.
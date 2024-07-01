---
title: Error Handling
description: Generates custom HTTP responses for exceptions and missing content.
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.ErrorHandling/" title="GenHTTP.Modules.ErrorHandling" icon="link" >}}
{{< /cards >}}

If an exception occurs while a request is handled, the server will render the
exception into a structured error object that informs the client about the issue.

## HTML Error Pages

To generate HTML error pages, you can add `ErrorHandler.Html()` to your services.

```csharp
var api = Layout.Create()
                .AddService<...>()
                .Add(ErrorHandler.Html());
```

## Custom Error Handling

If you would like to send custom HTTP responses for exceptions, you can add
the `ErrorHandler` concern with a custom `IErrorMapper`.

```csharp
var errorHandling = ErrorHandler.From(new MyErrorMapper());

var api = Layout.Create()
                .AddService<...>()
                .Add(errorHandling);

// ...

public class MyErrorMapper : IErrorMapper<Exception>
{

    public ValueTask<IResponse?> GetNotFound(IRequest request, IHandler handler)
    {
        // add your implementation here
        return request.Respond()
                      .Build();
    }
    
    public ValueTask<IResponse?> Map(IRequest request, IHandler handler, Exception error)
    {
        // add your implementation here
        return request.Respond()
                      .Build();
    }

}
```

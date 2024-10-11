---
title: Error Handling
description: Generates custom HTTP responses for exceptions and missing content.
cascade:
  type: docs
---

If an exceptions occurs while processing a request or a requested resource cannot be found,
the server will return a JSON object describing the error such as

```json
{
    "status": 404,
    "message": "The requested resource does not exist on this server"
}
```

## HTML Error Messages

To render error messages to HTML (so they can be viewed in a browser), you can use the built-in
`ErrorHandler.Html()`.

```csharp
var app = Layout.Create()
                // ...
                .Add(ErrorHandler.Html());
```

## Custom Error Handlers

To fully customize error handling, you can implement `IErrorMapper<T>` and pass an instance
of your mapper to the error handling concern. The following example will render any error
to a text formatted response:

```csharp
var errorHandler = ErrorHandler.From(new TextErrorMapper());

var app = Layout.Create()
                // ...
                .Add(errorHandler);

public class TextErrorMapper : IErrorMapper<Exception>
{

    public ValueTask<IResponse?> GetNotFound(IRequest request, IHandler handler)
    {
        var response = request.Respond()
                              .Status(ResponseStatus.NotFound)
                              .Content("Not found")
                              .Build();

        return new(response);
    }

    public ValueTask<IResponse?> Map(IRequest request, IHandler handler, Exception error)
    {
        var status = (error is ProviderException pe) ? pe.Status : ResponseStatus.InternalServerError;

        var response = request.Respond()
                              .Status(status)
                              .Content(error.Message)
                              .Build();

        return new(response);
    }

}
```

---
title: Error Handling
description: Generates custom HTTP responses for exceptions and missing content.
cascade:
  type: docs
---

If an exception occurs while a request is handled, the server will render the
exception into a HTML page that informs the client about the issue.

If you would like to send cutom HTTP responses for exceptions, you can add
the `ErrorHandler` concern with a custom `IErrorMapper`.

The following example will generate a JSON response for errors and missing
content:

```csharp
var errorHandling = ErrorHandler.From(new JsonErrorMapper());

var api = Layout.Create()
                .AddService<...>()
                .Add(errorHandling);

public record ErrorModel(String Message);

public class JsonErrorMapper : IErrorMapper<Exception>
{

    public ValueTask<IResponse?> GetNotFound(IRequest request, IHandler handler)
    {
        var errorModel = new ErrorModel("The requested content was not found");

        // hint: return null here to render the default error page of the server
        return new(GetResponse(request, ResponseStatus.NotFound, errorModel));
    }
    
    public ValueTask<IResponse?> Map(IRequest request, IHandler handler, Exception error)
    {
        var errorModel = new ErrorModel(error.Message);

        if (error is ProviderException providerException)
        {
            return new(GetResponse(request, providerException.Status, errorModel));
        }

        return new(GetResponse(request, ResponseStatus.InternalServerError, errorModel));
    }

    private static IResponse GetResponse(IRequest request, ResponseStatus status, ErrorModel model)
    {
        return request.Respond()
                      .Status(status)
                      .Content(new JsonContent(model, new()))
                      .Type(ContentType.ApplicationJson)
                      .Build()
    }

}
```

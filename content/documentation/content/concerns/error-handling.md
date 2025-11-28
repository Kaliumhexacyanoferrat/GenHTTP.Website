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
exception into a JSON response that informs the client about the issue.

## HTML Error Pages

To get a HTML error response instead of JSON, you can add the HTML error renderer to your app as a concern:

```csharp
var app = Layout.Create()
                .AddService<...>()
                .Add(ErrorHandler.Html());
```

## Custom Error Responses

If you would like to control the response generated for exceptions, you can implement a custom
`IErrorMapper` and add it to your app as needed. The following example will render any exception
to a text response:

```csharp
using GenHTTP.Api.Content;
using GenHTTP.Api.Protocol;

using GenHTTP.Modules.ErrorHandling;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.IO;

var errorHandling = ErrorHandler.From(new TextErrorMapper());

var api = Layout.Create()
                .AddService<...>()
                .Add(errorHandling);

public record ErrorModel(String Message);

public class TextErrorMapper : IErrorMapper<Exception>
{

    public ValueTask<IResponse?> GetNotFound(IRequest request, IHandler handler)
    {
        // hint: return null here to render the default error page of the server
        return new(GetResponse(request, ResponseStatus.NotFound, "The requested content was not found"));
    }

    public ValueTask<IResponse?> Map(IRequest request, IHandler handler, Exception error)
    {
        var errorModel = new ErrorModel(error.Message);

        if (error is ProviderException providerException)
        {
            return new(GetResponse(request, providerException.Status, error.ToString()));
        }

        return new(GetResponse(request, ResponseStatus.InternalServerError, error.ToString()));
    }

    private static IResponse GetResponse(IRequest request, ResponseStatus status, string message)
    {
        return request.Respond()
                      .Status(status)
                      .Content(message)
                      .Type(ContentType.TextPlain)
                      .Build();
    }

}
```

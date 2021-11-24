## Functional Handlers

With this module, requests can be handled in a functional manner, reducing
the boiler plate code to be written by a web application developer.

```csharp
using GenHTTP.Modules.Functional;

var handler = Inline.Create()
                    .Get("/increment", (i) => i + 1) // GET /increment?i=1
                    .Get("/decrement/:i", (i) => i - 1); // GET /decrement/2

Host.Create()
    .Handler(handler)
    .Run();
```

As with the [webservice module](./webservices), functions can use various
parameter and return types, including `IRequest`, `IResponse`, `IHandler` and
`Stream`. Both synchronous and `async` methods are supported.
---
title: Functional Handlers
cascade:
  type: docs
---

With this module, requests can be handled in a functional manner, reducing
the boiler plate code to be written by a web application developer.

> <span class="note">NOTE</span> Apps can quickly be created by using a [project template](./templates).

## Basic Structure

The following program will provide a simple web service to increment and
decrement given numbers.

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

## Further Resources

The following capabilities are shared by various application frameworks:

- [Serialization and deserialization](./conversion)
- [Parameter injection](./injection)
- [Results](./results)

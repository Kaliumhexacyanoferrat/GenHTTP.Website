---
title: Functional Handlers
description: Respond to HTTP requests in a functional manner with no boiler code.
weight: 2
cascade:
  type: docs
---

With this module, requests can be handled in a functional manner, reducing
the boilerplate code to be written by a web application developer.

## Hosting an API

To host an API using this framework you can create an `Inline` handler and add
your operations as needed. 

```csharp
using GenHTTP.Engine.Internal;

using GenHTTP.Modules.ApiBrowsing;
using GenHTTP.Modules.Functional;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.OpenApi;
using GenHTTP.Modules.Security;

var bookService = Inline.Create()                        
                        // GET http://localhost:8080/books/?page=1&pageSize=20
                        .Get((int page, int pageSize) => /* ... */)
                        // GET http://localhost:8080/books/4711
                        .Get(":id", (int id) => /* ... */) 
                        // PUT http://localhost:8080/books/
                        .Put((Book book) => /* ... */) 
                        // POST http://localhost:8080/books/
                        .Post((Book book) => /* ... */) 
                        // DELETE http://localhost:8080/books/4711
                        .Delete(":id", (int id) => /* ... */);

var api = Layout.Create()
                .Add("books", bookService)
                .Add(CorsPolicy.Permissive())
                .AddOpenApi() // http://localhost:8080/openapi.json
                .AddRedoc(); // http://localhost:8080/redoc/

await Host.Create()
          .Handler(api)
          .Development()
          .RunAsync();
```

## Further Resources

The following capabilities are shared by various application frameworks:

{{< cards >}}
{{< card link="../../concepts/definitions/" title="Method Definitions" icon="chip" >}}
{{< /cards >}}

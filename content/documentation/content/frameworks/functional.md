---
title: Functional Handlers
description: Respond to HTTP requests in a functional manner with no boiler code.
weight: 2
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Functional/" title="GenHTTP.Modules.Functional" icon="link" >}}
{{< /cards >}}

With this module, requests can be handled in a functional manner, reducing
the boilerplate code to be written by a web application developer.

{{< callout type="info" >}}
Apps can quickly be created by using a [project template](../../templates).
{{< /callout >}}

## Hosting an API

To host an API using this framework you can create an `Inline` handler and add
your operations as needed. 

```csharp
using GenHTTP.Engine.Internal;

using GenHTTP.Modules.Functional;
using GenHTTP.Modules.Layouting;

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
                .Add("books", bookService);

await Host.Create()
          .Handler(api)
          .Development()
          .Console()
          .RunAsync();
```

## Further Resources

The following capabilities are shared by various application frameworks:

{{< cards >}}
{{< card link="../../concepts/definitions" title="Method Definitions" icon="chip" >}}
{{< /cards >}}

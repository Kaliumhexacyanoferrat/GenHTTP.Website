---
title: Webservices
description: 'Provide REST based web services in C# that can be consumed by clients to retrieve a JSON, YMAL, or XML serialized result.'
weight: 1
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Webservices/" title="GenHTTP.Modules.Webservices" icon="link" >}}
{{< /cards >}}

The webservice module provides an easy way to implement RESTful services
that can be consumed by clients as needed.

{{< callout type="info" >}}
Webservices can quickly be created by using a [project template](../../templates).
{{< /callout >}}

## Hosting an API

To host an API using this framework you can define a new class that
hosts your operations as dedicated methods. The signature of those methods will
define how your API can be called via HTTP.

The following example shows how to define and host a service that can be used
to manage an entity (books in this case).

```csharp
using GenHTTP.Engine;

using GenHTTP.Api.Protocol;

using GenHTTP.Modules.Webservices;
using GenHTTP.Modules.Security;
using GenHTTP.Modules.Layouting;

public class BookService
{

    // GET http://localhost:8080/books/?page=1&pageSize=20
    [ResourceMethod]
    public List<Book> GetBooks(int page, int pageSize) { /* ... */ }

    // GET http://localhost:8080/books/4711
    [ResourceMethod(":id")]
    public Book? GetBook(int id) { /* ... */ }

    // PUT http://localhost:8080/books/
    [ResourceMethod(RequestMethod.PUT)]
    public Book AddBook(Book book) { /* ... */ }

    // POST http://localhost:8080/books/
    [ResourceMethod(RequestMethod.POST)]
    public Book UpdateBook(Book book) { /* ... */ }

    // DELETE http://localhost:8080/books/4711
    [ResourceMethod(RequestMethod.DELETE, ":id")]
    public Book? DeleteBook(int id) { /* ... */ }

}

var service = Layout.Create()
                    .AddService<BookService>("books")
                    .Add(CorsPolicy.Permissive());

await Host.Create()
          .Handler(service)
          .Development()
          .Console()
          .RunAsync();
```

## Further Resources

The following capabilities are shared by various application frameworks:

{{< cards >}}
{{< card link="../../concepts/definitions" title="Method Definitions" icon="chip" >}}
{{< /cards >}}

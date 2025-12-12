---
title: API Browsing
description: 'Serves applications such as Swagger UI or Redoc to visualize an Open API definition hosted by the server.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.ApiBrowsing/" title="GenHTTP.Modules.ApiBrowsing" icon="link" >}}
{{< /cards >}}

This module adds JavaScript based applications to visualize an Open API
definition such as [Swagger UI](https://swagger.io/tools/swagger-ui/), 
[Redoc](https://redocly.com/) or [Scalar](https://scalar.com/) to your app. All resources required by those
apps are hosted locally so no internet connection is required.

The following example creates an API, the corresponding Open API definition
and two endpoints to host Swagger and Redoc.

{{< tabs items="Webservices,Functional,Controllers" >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Api.Protocol;
  
  using GenHTTP.Engine.Internal;
  
  using GenHTTP.Modules.ApiBrowsing;
  using GenHTTP.Modules.Layouting;
  using GenHTTP.Modules.OpenApi;
  using GenHTTP.Modules.Practices;
  using GenHTTP.Modules.Webservices;
  
  var app = Layout.Create()
                  .AddService<BookService>("books")
                  .AddOpenApi()
                  .AddSwaggerUi()
                  .AddRedoc()
                  .AddScalar();
  
  await Host.Create()
            .Handler(app)
            .Defaults()
            .Development()
            .Console()
            .RunAsync();
  
  public record Book(int ID, string Title);
  
  public class BookService
  {
  
      [ResourceMethod]
      public List<Book> GetBooks() => [];
  
      [ResourceMethod(RequestMethod.Put)]
      public Book CreateBook(Book book) => book;
  
      [ResourceMethod(RequestMethod.Post)]
      public Book UpdateBook(Book book) => book;
  
      [ResourceMethod(RequestMethod.Delete)]
      public void DeleteBook(int id) { }
  
  }
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Engine.Internal;

  using GenHTTP.Modules.ApiBrowsing;
  using GenHTTP.Modules.Functional;
  using GenHTTP.Modules.Layouting;
  using GenHTTP.Modules.OpenApi;
  using GenHTTP.Modules.Practices;

  var bookApi = Inline.Create()
                  .Get(() => new List<Book>())
                  .Put((Book book) => book)
                  .Post((Book book) => book)
                  .Delete((int id) => { });

  var app = Layout.Create()
                  .Add("books", bookApi)
                  .AddOpenApi()
                  .AddSwaggerUi()
                  .AddRedoc()
                  .AddScalar();

  await Host.Create()
            .Handler(app)
            .Defaults()
            .Development()
            .Console()
            .RunAsync();

  public record Book(int ID, string Title);
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Api.Protocol;

  using GenHTTP.Engine.Internal;

  using GenHTTP.Modules.ApiBrowsing;
  using GenHTTP.Modules.Controllers;
  using GenHTTP.Modules.Layouting;
  using GenHTTP.Modules.OpenApi;
  using GenHTTP.Modules.Practices;

  var app = Layout.Create()
                  .AddController<BookController>("books")
                  .AddOpenApi()
                  .AddSwaggerUi()
                  .AddRedoc()
                  .AddScalar();

  await Host.Create()
            .Handler(app)
            .Defaults()
            .Development()
            .Console()
            .RunAsync();

  public record Book(int ID, string Title);

  public class BookController
  {

      [ControllerAction(RequestMethod.Get)]
      public List<Book> List() => [];

      [ControllerAction(RequestMethod.Put)]
      public Book Create(Book book) => book;

      [ControllerAction(RequestMethod.Post)]
      public Book Update(Book book) => book;

      [ControllerAction(RequestMethod.Delete)]
      public void Delete(int id) { }

  }
  ```
{{< /tab >}}

{{< /tabs >}}

After running this sample you can view http://localhost:8080/swagger/, http://localhost:8080/redoc/
and http://localhost:8080/scalar/ in your browser.

## Referencing a Definition

By default, the applications will use `../openapi.json` to
reference to the Open API definition to be used. There is no
magic link between the API browser and the Open API handler, so you 
need to pass the correct link depending on the structure of your 
application. In the following example, docs and the API are separated
so another path is required and needs to be passed:

```csharp
var bookApi = Inline.Create()
                    .Get(() => new List<Book>())
                    .Put((Book book) => book)
                    .Post((Book book) => book)
                    .Delete((int id) => { });

var api = Layout.Create()
                .Add("books", bookApi)
                .AddOpenApi();

var app = Layout.Create()
                .Add(["api", "v1"], api)
                .AddSwaggerUi(url: "../api/v1/openapi.json");
```

While you can pass an absolute URL, we recommend to pass a relative
URL to avoid issues with CORS and your reverse proxy setup.

## Adjusting the Path

By default, the applications will either be registered at `/swagger/` or
`/redoc/`, relative to the path of the current layout they are added to.
This path can be adjusted as needed:

```csharp
.AddSwaggerUi(segment: "docs");
```

## Adjusting Meta Data

There are overloads to adjust the meta data of the generated
HTML index file. Currently, only the title can be changed.

```csharp
.AddSwaggerUi(title: "Book API");
```

## Hiding in Production

You might want use this feature only during development
and disable it in production. There is no built-in way
to achieve this, but you can add the application conditionally,
e.g. depending on the project configuration:

```csharp
#if DEBUG
.AddSwaggerUi();
#endif
```

## Advanced Usage

The examples above show the typical use case of adding
an API browser to a layout. For full access to the handlers,
you may also directly create and link them.

```csharp
var swagger = ApiBrowser.SwaggerUI()
                        .Url("https://my.company.com/api/openapi.json");

await Host.Create()
          .Handler(swagger)
          .Defaults()
          .RunAsync();
```

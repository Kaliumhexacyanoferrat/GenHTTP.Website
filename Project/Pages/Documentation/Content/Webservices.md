## Webservices

The resource handler shipped with the [Webservices](https://www.nuget.org/packages/GenHTTP.Modules.Webservices/)
module allows to easily implement RESTful services. The
concept is very similar to popular webservice frameworks
such as [JAX-RS](https://github.com/jax-rs):

```csharp
using GenHTTP.Modules.Webservices;

public class BookResource
{

    [ResourceMethod]
    public List<Book> GetBooks(int page, int pageSize) { /* ... */ }

    [ResourceMethod(":id")]
    public Book? GetBook(int id) { /* ... */ }

    [ResourceMethod(RequestMethod.PUT)]
    public Book AddBook(Book book) { /* ... */ }
                        
    [ResourceMethod(RequestMethod.POST)]
    public Book UpdateBook(Book book) { /* ... */ }

    [ResourceMethod(RequestMethod.DELETE, ":id")]
    public Book? DeleteBook(int id) { /* ... */ }

}

var service = Layout.Create().AddService<BookResource>("books");

Host.Create()
    .Handler(service)
    .Run();
```

The service will be available at http://localhost:8080/books.
As the functionality is provided on handler level,
all other concerns such as authentication or CORS can
be implemented using regular server mechanisms. 

By default, parameter values (within the path) are expected
to be alphanumeric. If needed, a custom pattern can be specified
to define the format accepted by the service:

```csharp
[ResourceMethod("(?<id>[0-9]{12,13})")] // EAN-13
public Book? GetBook(int id) { /* ... */ }
```

Because the resource hander needs to utilize regular expressions
and reflection to achieve the required functionality, its performance
will be slow compared to `IHandler` instances
providing the same functionality.

The implementation supports all typical features such as
serialization and deserialization of complex types, simple
types, enums, streams, raw `IRequest`, `IHandler`, and `IResponseBuilder`
arguments, query parameters, path parameters or exception handling:

```csharp
[ResourceMethod(RequestMethod.PUT)]
public Stream Stream(Stream input) { /* ... */ }

[ResourceMethod]
public IResponseBuilder RequestResponse(IRequest request, IHandler handler) { return request.Respond() /* ... */; }

[ResourceMethod]
public void Exception() => throw new ProviderException(ResponseStatus.AlreadyReported, "Already reported!");
```

By default, resources will consume and produce JSON and XML entities. To
customize this behavior, add a custom `ISerializationFormat`
implementation to the serialization registry used within your resources:

```csharp
var registry = Serialization.Default().Add(new FlexibleContentType("application/protobuf"), new ProtobufFormat());

var resource = Resource.From<BookResource>().Formats(registry);

var service = Layout.Create().Add("books", resource);
```
## Conversion

As content serialization and deserialization is required by various application
frameworks (such as [webservices](./webservices), [controllers](./controllers), or [functional handlers](./functional)),
there is a modular approach that can be used where needed.

Serialization is provided by a `SerializationRegistry` that can be passed into
the application framework builder. By default, handlers may consume and produce JSON and XML entities.
To customize this behavior, add a custom `ISerializationFormat`
implementation to the serialization registry.

The following example shows how to add Protocol Buffers support to a webservice resource: 

```csharp
var registry = Serialization.Default()
                            .Add(new FlexibleContentType("application/protobuf"), new ProtobufFormat());

var resource = ServiceResource.From<BookResource>()
                              .Formats(registry);

var service = Layout.Create().Add("books", resource);
```
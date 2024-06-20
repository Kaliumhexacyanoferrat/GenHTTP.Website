---
title: Conversion
cascade:
  type: docs
---

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

var api = Layout.Create()
                .AddService<BookResource>("books", resource, formatters: registry);
```

## Formatting

Similar to serialization, there is a mechanism which allows to control how types that are used as parameters
in your services a serialized and deserialized. The default configuration allows you to use types like
`Guid`, `DateOnly`, enumerations or simple types such as `int` or `string` as path parameters and within
form encoded content.

Formatting is provided by a `FormatterRegistry` that can be passed whereever you create a service, inline
or controller instance. To customize this behavior, add a custom `IFormatter` implementation to the default
registry:

```csharp
var registry = Formatting.Default()
                         .Add(new MyTypeFormat());

var controllers = Layout.Create()
                        .Add<MyController>("my", formatters: registry);
```

This will allow you to use `MyType` as a path parameter or within form encoded data in your controller.

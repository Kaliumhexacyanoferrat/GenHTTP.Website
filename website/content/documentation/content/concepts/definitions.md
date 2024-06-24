---
title: Method Definitions
cascade:
  type: docs
---

All API frameworks share a common base provided by the `Reflection` and `Conversion` modules which
define some of the capabilities that can be used when defining an API method
signature.

## Injection

This section describes how parameters of your methods are initialized and which
kind of types can be used.

### Primitives

By default, the following types can be used as parameters within a method definition: 
`string`, `bool`, `enum`, `Guid`, `DateOnly` and any other primitive type (such as
`int`).

{{< tabs items="Webservices,Functional,Controllers" >}}

{{< tab >}}
  ```csharp
  [ResourceMethod]
  public int Length(string text) => text.Length;
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  .Get((string text) => text.Length)
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  public int Index(string text) => text.Length;
  ```
{{< /tab >}}

{{< /tabs >}}

Parameters can be declared nullable (e.g. `int?`) and will be initialized with `null` if not present.
If not declared nullable, parameters will be initialized with `default(T)` if not present.

By default, parameters are read from the request query (`?text=abc`) or from a [form encoded](#html-forms) body.

If you would like to read the parameter directly from the request body, you can mark it with the `[FromBody]` attribute.

{{< tabs items="Webservices,Functional,Controllers" >}}

{{< tab >}}
  ```csharp
  [ResourceMethod(RequestMethod.PUT)]
  public int Length([FromBody] string text) => text.Length;
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  .Put(([FromBody] string text) => text.Length)
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  [ControllerAction(RequestMethod.PUT)]
  public int Index([FromBody] string text) => text.Length;
  ```
{{< /tab >}}

{{< /tabs >}}

To read the parameter from the request path, use the appropriate method provided by the application framework.

{{< tabs items="Webservices,Functional,Controllers" >}}

{{< tab >}}
  ```csharp
  [ResourceMethod(RequestMethod.DELETE, ":id")]
  public void Delete(int id) { /* ... */ }
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  .Delete("/:id", (int id) => { /* ... */ })
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  [ControllerAction(RequestMethod.DELETE)]
  public int Delete([FromPath] int id) { /* ... */ }
  ```
{{< /tab >}}

{{< /tabs >}}

Some frameworks allow to further restrict path parameters using a regular expression.

{{< tabs items="Webservices,Functional" >}}

{{< tab >}}
  ```csharp
  [ResourceMethod("(?<ean13>[0-9]{12,13})")]
  public Book? GetBook(int ean13) { /* ... */ }
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  .Get("/books/?<ean13>[0-9]{12,13})", (int ean13) => { /* ... */ })
  ```
{{< /tab >}}

{{< /tabs >}}

### Complex Types

When using a complex type in a parameter declaration, the value will be [deserialized](#serialization-formats) from the
request body. By default, handlers will accept content declared as XML, JSON or Form Encoded. If
the client does not declare the `Content-Type`, the server will try to treat the body as JSON.

{{< tabs items="Webservices,Functional,Controllers" >}}

{{< tab >}}
  ```csharp
  [ResourceMethod(RequestMethod.POST)]
  public void Save(MyClass data) { /* ... */ }
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  .Post((MyClass data) => { /* ... */ })
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  [ControllerAction(RequestMethod.POST)]
  public int Save(MyClass data) { /* ... */ }
  ```
{{< /tab >}}

{{< /tabs >}}

### HTML Forms

### Request Injection

### Handler Injection

### Streaming

### Custom Injection

### User Injection

## Response Generation

### Primitives

### Complex Types

### Custom Responses

### Results

### Handlers

### Streams

### Empty Responses

## Behavior

### Serialization Formats

### Asynchronous Execution

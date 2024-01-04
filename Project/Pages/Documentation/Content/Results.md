## Results

When writing [webservices](./webservices), [controllers](./controllers), or [functional handlers](./functional),
you might want to adjust the generated response without the need of bulding an `IResponse` yourself. The `Result<T>`
allows you to keep strong return types while being able to modify the generated response:

```csharp
[ResourceMethod(RequestMethod.PUT)]
public Result<Book> AddBook(Book book)
{ 
  var created = ... // do work

  return new(created).Status(ResponseStatus.Created);
}
```

The `Result<T>` follows the semantics of the `IResponseBuilder` so you can adjust the generated response as needed.
This does not only work for data structures, but also for special types such as streams.
## Layouting

Layouts allow the assembly of a web application from many different parts. This
allows you to use different handlers for different kind of work.

```csharp
var shop = Layout.Create()
                 .Add("checkout", Page.From(...));

var api = Layout.Create()
                 .AddService<CartResource>("cart");

var project = Layout.Create()
                    .Index(Page.From(...))
                    .Add("shop", shop) // e.g. http://localhost:8080/shop/checkout
                    .Add("api", api); // e.g. http://localhost:8080/api/cart/items

var website = Website.Create()
                     .Content(project);
```
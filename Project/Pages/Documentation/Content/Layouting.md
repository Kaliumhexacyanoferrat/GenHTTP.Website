## Layouting

Layouts allow the assembly of a web application from different parts. This
allows you to use different handlers for different kind of work.

```csharp
var shop = Website.Create(); // ...

var adminArea = Website.Create(); // ...

var api = Layout.Create()
                 .AddService<CartResource>("cart");

var project = Layout.Create()
                    .Add("api", api); // e.g. http://localhost:8080/api/cart/items
                    .Add("admin", adminArea) // e.g. http://localhost:8080/admin/users
                    .Add(shop) // e.g. http://localhost:8080/checkout
                    .Index(Page.From(...));
```

## Controllers & MVC

The [Controllers](https://www.nuget.org/packages/GenHTTP.Modules.Controllers/) module
adds a thin layer that allows to serve content from controller classes. Combined
with the [Website](./websites) framework and a renderer of your choice,
this allows to implement MVC-style web applications. Nevertheless, controllers can 
be used to generate any content, so the functionality is not limited to generate pages 
for websites.

> <span class="note">NOTE</span> New MVC websites can quickly be created by using a [project template](./templates).

In comparison to [ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-3.1&tabs=visual-studio),
the implementation tries to use less conventions and hidden logic, keeping the learning
curve rather low.

## Creating a MVC Website

The following snippet shows a basic example on how to implement a MVC style web application
using the controller module. This example shows how to render and modify data records.

```csharp
// Model
public record Book(int ID, string Title);

// Controller
public class BookController
{
    private static readonly List<Book> _Books = new()
    {
        new Book(1, "Lord of The Rings")
    };

    public IHandlerBuilder Index()
    {
        return ModScriban.Page<ViewModel<List<Book>>>(Resource.FromAssembly("BookList.html"), (r, h) => new(new ViewModel<List<Book>>(r, h, _Books)))
                         .Title("Book List");
    }

    public IHandlerBuilder Create()
    {
        return ModScriban.Page(Resource.FromAssembly("BookCreation.html"))
                         .Title("Add Book");
    }

    [ControllerAction(RequestMethod.POST)]
    public IHandlerBuilder Create(string title)
    {
        var book = new Book(_Books.Max(b => b.ID) + 1, title);

        _Books.Add(book);

        return Redirect.To("{index}/", true);
    }

    public IHandlerBuilder Edit([FromPath] int id)
    {
        var book = _Books.Where(b => b.ID == id).First();
        
        return ModScriban.Page<ViewModel<Book>>(Resource.FromAssembly("BookEditor.html"), (r, h) => new(new ViewModel<Book>(r, h, book)))
                         .Title(book.Title);
    }

    [ControllerAction(RequestMethod.POST)]
    public IHandlerBuilder Edit([FromPath] int id, string title)
    {
        var book = _Books.Where(b => b.ID == id).First();

        var index = _Books.IndexOf(book);

        _Books[index] = book with { Title = title };

        return Redirect.To("{index}/", true);
    }

    [ControllerAction(RequestMethod.POST)]
    public IHandlerBuilder Delete([FromPath] int id)
    {
        _Books.RemoveAll(b => b.ID == id);

        return Redirect.To("{index}/", true);
    }

}

// Website setup
static int Main(string[] args)
{
    var app = Layout.Create()
                    .AddController<BookController>("books")
                    .Index(Page.From("Welcome to the Book Manager!").Title("Home"));

    var theme = Theme.Create()
                     .Title("Book Manager");

    var website = Website.Create()
                         .Theme(theme)
                         .Content(app);

    return Host.Create()
               .Handler(website)
               .Defaults()
               .Development()
               .Console()
               .Run();
}
```

The example is available as a downloadable [sample project](/downloads/GenHTTP-Examples-Controllers.zip) as well.

You might notice that compared to ASP.NET Core MVC, the code is quite verbose (e.g.
the Scriban stuff instead of just `View`), but you can also see what is actually executed,
making it easier to understand. A larger application would probably derive some
`BaseController` with a method like the following:

```csharp
protected IHandlerBuilder View<T>(string name, string title, T data) {
    return ModScriban.Page<ViewModel<T>>(Resource.FromAssembly($"{name}.html"), (r, h) => new(new ViewModel<T>(r, h, data)))
                     .Title(title);
}
```

## Content Discovery

When you run the sample project, you will notice, that there is an automatically
generated menu that has been derived from our controller class:

![automatically generated menu](/images/controller_menu.png)

This is possible because the controller handler (as any other handler of the GenHTTP
framework) declares the content elements it provides as meta information. From this
information the sitemap (http://localhost:8080/sitemap.xml is generated as well).

If you do not want a page to show up in the sitemap or menu, you can mark it hidden:

```csharp
[ControllerAction(RequestMethod.GET, IgnoreContent = true)]
public IHandlerBuilder Create() { /* ... */ }
```

For dynamic content (such as the books in our example), you can specify an `IContentHint`
implementation which makes the content discoverable to the framework:

```csharp
[ControllerAction(RequestMethod.GET, ContentHints = typeof(BookContentHints))]
public IHandlerBuilder Edit([FromPath] int id) { /* ... */ }

public class BookContentHints : IContentHints
{

    public List<ContentHint> GetHints(IRequest request)
    {
        var result = new List<ContentHint>();

        foreach (var book in BookController.Books)
        {
            result.Add(new ContentHint() { { "id", book.ID } });
        }

        return result;
    }

}
```

## URL Patterns

As with ASP.NET Core MVC, the endpoints of your controller methods will automatically
be derived by analyzing your class. The following table shows the rules that are applied
to map an URL:

| Method                      | Endpoint      |
| -------------               | ------------- |
| `Index()`                   | `/controller/` |
| `Action()`                  | `/controller/action/` |
| `Action([FromPath] int id)` | `/controller/action/:id/` |
| `Action(int id)`            | `/controller/action/?id=:id` |
| `LongAction()`              | `/controller/long-action/` |

## Advanced Method Definitions

As with the webservice module, the controller allows to inject additional objects
from the request context such as the `IRequest`, the `IHandler` or the `Stream` read
from the request body. The result of your method can be `void`, `IResponse`,
`IResponseBuilder`, `IHandler` or `IHandlerBuilder` (either `async` or not).

As your methods are allowed to return any `IHandlerBuilder`, you can also return
more complex types like a `Layout`, `ReverseProxy` or a `DirectoryListing`. Content discovery and
routing will work in this case as well.
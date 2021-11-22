## Websites

The website handler allows your to create themed web applications in a simple manner.

> <span class="note">NOTE</span> New websites can quickly be created by using a [project template](./templates).

## Creating a Website

The following code snippet will start a very basic web application with two pages:

```csharp
var project = Layout.Create()
                    .Index(Page.From("Home", "Hello World!"))
                    .Add("legal", Page.From("Legal", "This is the legal page"));

var website = Website.Create()
                     .Theme(Theme.Create())
                     .Content(project);
                        
Host.Create()
    .Defaults()
    .Console()
    .Handler(website)
    .Run();
```

 After starting the project, you can access the web application by entering http://localhost:8080
in the address line of your browser.

## Themes

The GenHTTP SDK ships with some [default themes](https://github.com/Kaliumhexacyanoferrat/GenHTTP.Themes) 
that can be used with the website handler.

After installing the theme you would like to use, configure and pass a theme instance to the Website builder:

```csharp
var theme = Theme.Create()
                 .Title("Website Title");

var website = Website.Create()
                     .Content(...)
                     .Theme(theme);
```

Depending on the theme, there will be different features availabe for configuration (such as footer menus or social buttons).

To provide a custom theme, you will need to implement the
[ITheme](https://github.com/Kaliumhexacyanoferrat/GenHTTP/blob/master/API/Content/Websites/ITheme.cs) interface.

The main component of every theme is the template that is used to embed the generated pages into. The template can be rendered
using a templating engine of your choice. For a Scriban-based example, see the
[template of the Lorahost theme](https://github.com/Kaliumhexacyanoferrat/GenHTTP.Themes/blob/master/Lorahost/Template.html).

## Adding Pages

To add your pages to the web application, there are different template languages available
to be used: [Scriban](https://www.nuget.org/packages/GenHTTP.Modules.Scriban/),
[Razor](https://www.nuget.org/packages/GenHTTP.Modules.Razor/) and [Markdown](https://www.nuget.org/packages/GenHTTP.Modules.Markdown/).

> <span class="note">NOTE</span> To build more complex websites, the [MVC pattern](./controllers) is recommended. 

Create a new HTML file within the tree of your project and mark it either as an embedded resource
or as content that will be copied to the output directory of your project. The page can the be
added using the appropriate module:

```csharp
website.Add("shop", ModScriban.Page(Resource.FromFile("Shop.html"))); // or ModRazor, ModMarkdown
```

This will load the given Scriban template from the specified resource and render it to the
requesting client, if http://localhost:8080/shop is called.

You may provide a model used to render your pages based on the current request
by providing a factory method:

```csharp
// within the template, you might use "{{ data.total_items }}" (or "@Model.Data.TotalItems" in Razor) to access your view model
website.Add("shop", ModScriban.Page<ViewModel<Basket>>(Resource.FromFile("./Shop.html"), LoadBasket));

public record Basket(int TotalItems);

private async ValueTask<ViewModel<Basket>> LoadBasket(IRequest request, IHandler handler) 
{
    var basket = await ...; // load the basket from your data source

    return new ViewModel<Basket>(request, handler, basket);
}
```

> <span class="note">NOTE</span> View model classes must override <i>GetHashCode()</i> to allow changes to be detected. Using C#'s <i>record</i> will do this for you automatically. 

To serve a static page instead of a rendered one, you can use `Page.From(Resource.FromFile(...))` to
create a page from a given [Resource](./resources).

## Hot Reload

The [auto reload module](https://www.nuget.org/packages/GenHTTP.Modules.AutoReload/) improves 
the website development experience by automatically reloading the currently shown page when 
changes are detected. As the change detection regularly polls the website to detect modifications,
it should be used in development mode only.

```csharp
#if DEBUG
website.AutoReload();
#endif
```

## Routing

Within your pages, you can access the routing context to generate relative paths
to other pages or parts of your web application. Example:

```html
<a href="{{ route 'sitemap.xml' }}">Sitemap</a> <!-- Scriban -->
```

```html
<a href="@Model.Route("sitemap.xml")">Sitemap</a> <!-- Razor -->
```

 The routing mechanism will automatically find the responsible handler for the requested
route within the handler hierarchy and generate an URL that can be used by the
browser to navigate to this location.

Some handlers may provide symbolic routes that can be used to access the requested content:

| Handler        | Route           | Description  |
| ------------- |-------------| -----|
| [Layout](./layouting)     | `{index}` | The index of the layout (if any) |
| [Layout](./layouting)     | `{fallback}` | The fallback of the layout (if any) |
| [Website](./websites)     | `{website}` | The root of the website |
| [Controller](./controllers)     | `{controller}` | The root of the controller |
| [Controller](./controllers)     | `{index}` | The index route of the controller (if any) |

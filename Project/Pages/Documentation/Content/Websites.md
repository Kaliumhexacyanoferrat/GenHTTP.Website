## Websites

The website handler allows your to create themed web applications in a simple manner.
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

Create a new HTML file within the tree of your project and mark it either as an embedded resource
or as content that will be copied to the output directory of your project. The page can the be
added using the appropriate module:

```csharp
website.Add("shop", ModScriban.Page(Data.FromResource("Shop.html"))); // or ModRazor, ModMarkdown
```

This will load the given Scriban template from the specified resource and render it to the
requesting client, if http://localhost:8080/shop is called.

You may provide a custom model used to render your pages based on the current request
by providing a factory method:

```csharp
public class ShopModel : PageModel
{

    public Basket Basket { get; }

    public ShopModel(IRequest request, IHandler handler, Basket basket) : base(request, handler)
    {
        Basket = basket;
    }
                        
}

// within the template, "basket" will be available as a Scriban property 
website.Add("shop", ModScriban.Page(Resource.FromFile("./Shop.html"), (request, handler) => new ShopModel(request, handler, LoadBasket())));
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

Some handlers may provide symbolic routes that can be used to access the requested content.
For example, the layout handler exposes the symbolic routes `{index}` and `{fallback}`
and the website handler exposes `{website}` to navigate to the root of the website.
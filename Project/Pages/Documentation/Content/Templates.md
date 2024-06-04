## Templates

Templates allow to create new projects based on the GenHTTP SDK
(such as webservices or websites) in a couple of minutes. To
install the project templates in your environment run the following
command in your terminal:

```bash
dotnet new -i GenHTTP.Templates
```

After the templates have been installed, new projects can be created
using the following commands:

```bash
mkdir AppName
cd AppName
dotnet new <template-name>
```

If installed, the templates will also show up in Visual Studio and can
be used from there to quickly create new projects:

![GenHTTP template projects in Visual Studio](/images/templates.png)

The following templates are available to be used:

| Template      | Description  | 
| ------------- |------------- | 
| `genhttp-webservice` | A project that will host a new [REST web service](./webservices). |
| `genhttp-webservice-minimal` | A project that will host a minimal web service in a single file using the [functional module](./functional). |
| `genhttp-website` \[[deprecated](https://github.com/Kaliumhexacyanoferrat/GenHTTP/issues/496)\] | A [website](./websites), mainly for static content (such as a business website). |
| `genhttp-website-mvc-razor` \[[deprecated](https://github.com/Kaliumhexacyanoferrat/GenHTTP/issues/496)\] | Dynamic website using the [MVC pattern](./controllers) and [Razor](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-5.0) as a templating engine. |
| `genhttp-website-mvc-scriban` \[[deprecated](https://github.com/Kaliumhexacyanoferrat/GenHTTP/issues/496)\] | Dynamic website using the [MVC pattern](./controllers) and [Scriban](https://github.com/scriban/scriban/) as a templating engine. |
| `genhttp-website-static` | Serves a [static website](./static-websites) from the file system. |
| `genhttp-spa` | Serves the distribution files of a [Single Page Application (SPA)](./single-page-applications). |

## Updating Templates

To update your locally installed project templates, run the following
command in your terminal:

```bash
dotnet new update
```
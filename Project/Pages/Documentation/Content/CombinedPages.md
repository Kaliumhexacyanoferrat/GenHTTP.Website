## Combined Pages

This provider allows to assemble different kind of content into a single
web page to be delivered. Rendering engines and technologies can be mixed
as needed.

```csharp
using GenHTTP.Modules.Markdown;
using GenHTTP.Modules.Scriban;

var shop = CombinedPage.Create()
                       .Title("Webshop")
                       .Description("Shop fancy stuff online!")
                       .AddMarkdown(Resource.FromFile("./marketing-campaign.md"))
                       .AddScriban(Resource.FromFile("./shop.html"), async (r, h) => await ListShopItems(r, h))
                       .Add("Copyright 2021");

var project = Layout.Create()
                    .Add("shop", shop)

var website = Website.Create()
                     .Content(project);
```
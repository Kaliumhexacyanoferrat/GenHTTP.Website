---
title: Redirects
description: 'Renders HTML pages from template files'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Pages/" title="GenHTTP.Modules.Pages" icon="link" >}}
{{< /cards >}}

The pages module allows the server to generate HTML pages (e.g. error messages or the directory listing) and can be
used to render custom pages from a template, using [Cottle](http://r3c.github.io/cottle/) as a rendering engine. Support
for rendering websites in GenHTTP is minimalistic, so you should not use this for a website project.

## Page Rendering

The following example will show you how to use a Cottle template file to generate a web page
with dynamic content via GenHTTP.

{{< tabs items="Source,Template.html" >}}

{{< tab >}}
  ```csharp
  using Cottle;
  
  using GenHTTP.Api.Protocol;
  
  using GenHTTP.Engine.Kestrel;
  
  using GenHTTP.Modules.Functional;
  using GenHTTP.Modules.IO;
  using GenHTTP.Modules.Pages;
  using GenHTTP.Modules.Practices;
  
  var template = Renderer.From(Resource.FromAssembly("Template.html").Build());
  
  var app = Inline.Create()
                  .Get(async (IRequest request) =>
                  {
                      var data = new Dictionary<Value, Value>
                      {
                          ["users"] = Value.FromEnumerable(GetUsers())
                      };
  
                      return request.GetPage(await template.RenderAsync(data));
                  });
  
  await Host.Create()
            .Handler(app)
            .Defaults()
            .Development()
            .Console()
            .RunAsync();
  
  static List<Value> GetUsers()
  {
      var result = new List<Value>();
  
      result.Add(Value.FromDictionary(new Dictionary<Value, Value>()
      {
          ["id"] = 1,
          ["name"] = "Some User"
      }));
  
      return result;
  }
  ```
{{< /tab >}}

{{< tab >}}
  ```html
  <!DOCTYPE html>
  <html lang="en">
  <head>
      <title>Users</title>
  </head>
  <body>
  
  <table>
      <tr>
          <th>ID</th>
          <th>Name</th>
      </tr>
      {for user in users:
      <tr>
          <td>{ user.id }</td>
          <td>{ user.name }</td>
      </tr>
      }
  </table>
  
  </body>
  </html>
  ```
{{< /tab >}}

{{< /tabs >}}

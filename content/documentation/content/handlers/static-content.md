---
title: Static Resources
description: 'Provide resources stored on the file system or within an assembly via HTTP.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.IO/" title="GenHTTP.Modules.IO" icon="link" >}}
{{< /cards >}}

To provide static resources required by your application, add them to your project
and mark them either as `Content` or as an `Embedded Resource`. To serve those files,
you can use the `Resources` factory class:

```csharp
var layout = Layout.Create();

// serve all embedded resources in the "Resources" sub folder of your project
var tree = ResourceTree.FromAssembly("Resources");

// serve all files in the given folder
var tree = ResourceTree.FromDirectory("./Resources");

layout.Add("res", Resources.From(tree));

await Host.Create()
          .Handler(layout)
          .RunAsync();
```

For example, a stylesheet named `main.css` in the `styles` subfolder would be made available at
http://localhost:8080/res/styles/main.css.

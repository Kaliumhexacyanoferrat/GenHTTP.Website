---
title: Content
description: 'Provides resources such as files or text'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.IO/" title="GenHTTP.Modules.IO" icon="link" >}}
{{< /cards >}}

This handler provides the content of a [resource](../../concepts/resources/) such
as a file or a text resource to a requesting client. In contrast
to [downloads](../downloads/), the handler does not instruct the
client to download and store the file but just serves it.

```csharp
using GenHTTP.Engine.Internal;

using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.IO;

var resource = Resource.FromString("Hello World!"); // or FromFile(), FromAssembly() ...

var content = Content.From(resource);

var app = Layout.Create()
                .Add("hello", content); // http://localhost:8080/hello

await Host.Create()
          .Handler(app)
          .RunAsync();
```

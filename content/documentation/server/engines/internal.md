---
title: Internal
weight: 1
description: 'The default HTTP engine implemented by the GenHTTP framework itself.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Core/" title="GenHTTP.Core" icon="link" >}}
{{< /cards >}}

This is the default HTTP implementation provided by the GenHTTP framework. It
relies only on code written in C# and tries to reduce dependencies to third-party
libraries and the environment it runs in as much as possible, therefore making it
a good choice for embedding a webservice into another application (e.g. WPF or Windows Forms)
or hosting a small service as a Docker container.

```csharp
using GenHTTP.Engine.Internal;

await Host.Create()
          .Handler(...)
          .RunAsync();
```

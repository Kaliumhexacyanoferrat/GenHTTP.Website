﻿---
title: Defaults
description: Automatically configures your web server for performance and security.
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Practices/" title="GenHTTP.Modules.Practices" icon="link" >}}
{{< /cards >}}

The `Defaults()` method provided by the practices
module adds some useful concerns to your web server to enable features such as 
[compression](../compression/) or [client side caching](../client-caching-validation/).
This way, you will have a well configured server instance without the need
of adding everything by yourself.

```csharp
await Host.Create()
          .Handler(...)
          .Defaults()
          .RunAsync();
```

If you would like to opt out of a default feature, you may pass a 
flag as needed:

```csharp
await Host.Create()
          .Handler(...)
          .Defaults(compression: false)
          .RunAsync();
```

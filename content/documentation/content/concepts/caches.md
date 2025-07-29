---
title: Caches
description: Different backends allowing to store computation heavy work for improved performance.
weight: 7
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Caching/" title="GenHTTP.Modules.Caching" icon="link" >}}
{{< /cards >}}

Caches can be used by handlers such as the [cached content](../../concerns/server-caching/) concern
to store and serve intermediate results for improved performance. The SDK ships with
backends for local memory and file system based storage. Custom backends can be
provided by implementing the [ICache](https://github.com/Kaliumhexacyanoferrat/GenHTTP/blob/master/API/Content/Caching/ICache.cs)
interface.

```csharp
var memoryCache = Cache.Memory<MyClass>();

var tempFileCache = Cache.TemporaryFiles<MyClass>();

var persistentFileCache = Cache.FileSystem<MyClass>("./cache");
```

---
title: Caches
description: Different backends allowing to store computation heavy work for improved performance.
weight: 5
cascade:
  type: docs
---

Caches can be used by handlers such as the [cached content](./server-caching) concern
to store and serve intermediate results for improved performance. The SDK ships with
backends for local memory and file system based storage. Custom backends can be
provided by implementing the [ICache](https://github.com/Kaliumhexacyanoferrat/GenHTTP/blob/master/API/Content/Caching/ICache.cs)
interface.

```csharp
var memoryCache = Cache.Memory<MyClass>();

var tempFileCache = Cache.TemporaryFiles<MyClass>();

var persistentFileCache = Cache.FileSystem<MyClass>("./cache");
```

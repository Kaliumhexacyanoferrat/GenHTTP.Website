---
title: Load Balancer
cascade:
  type: docs
---

Allows to distribute incoming requests to multiple nodes, either by using
a reverse proxy or by redirecting the client.

```csharp
var workers = LoadBalancer.Create()
                          .Proxy("http://worker-1")
                          .Proxy("http://worker-2");

var cdn = LoadBalancer.Create()
                      .Redirect("https://cdn1.domain.com")
                      .Redirect("https://cdn2.domain.com");

var app = Layout.Create()
                .Add("api", workers)
                .Add("content", cdn);
```

If needed, the incoming request can get analyzed to determine the nodes with
the highest priority that should handle the request (e.g. to prefer nodes
near to the requesting client).

```csharp
LoadBalancer.Create().Proxy(..., (r) => Priority.High);
```

Additionally, the builder accepts any `IHandler` instance as a node, allowing
to extend the functionality where needed. For example, the following snippet would
distribute the load to two different, local drives.

```csharp
LoadBalancer.Create()
            .Add(Resources.From(ResourceTree.FromDirectory("/mnt/storage1/files/")))
            .Add(Resources.From(ResourceTree.FromDirectory("/mnt/storage2/files/")));
```

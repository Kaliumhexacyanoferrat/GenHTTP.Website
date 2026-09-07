---
title: Engines
weight: 1
description: 'Describes the webserver engines available to run GenHTTP projects with (such as Kestrel or Ioxide).'
cascade:
  type: docs
---

GenHTTP is both a HTTP server implementation and a web service application framework.
Depending on your requirements, the underlying HTTP engine can be replaced with another web server.

The acceptance tests of the project ensure that you can replace the engine without any further
adjustments to your application code - so basically all you do is changing the core nuget package
and the `Host` namespace you import.

## Choosing an Engine

| Engine                  | Package                | Good fit for                                                                  |
|-------------------------|------------------------|-------------------------------------------------------------------------------|
| [Internal](./internal/) | `GenHTTP.Core`         | Embedding into another application, small Docker containers, few dependencies |
| [Kestrel](./kestrel/)   | `GenHTTP.Core.Kestrel` | Edge servers, security requirements, supports HTTP/2 and HTTP/3               |
| [Ioxide](./ioxide/)     | `GenHTTP.Core.Ioxide`  | High performance on Linux using `io_uring`, supports HTTP/2 and HTTP/3        |

## Detecting the Engine

The engine a server runs on is exposed as `IServer.ServerEngine`, a `ServerEngine` value
(`Internal`, `Kestrel`, `Ioxide` or `Custom`). Handlers and concerns receive the server instance in
their `PrepareAsync(IServer server)` call, so a module can select an engine-specific code path during
preparation - this is how the [files handler](../../content/handlers/files/#ioxide-engine) switches to
its native implementation on Ioxide.

```csharp
public ValueTask PrepareAsync(IServer server)
{
    _optimized = server.ServerEngine == ServerEngine.Ioxide;
    return ValueTask.CompletedTask;
}
```

A module must provide its functionality independently of the underlying engine and must not fail for
an engine it does not recognize, so this field is intended for optional optimizations rather than for
business logic.

## Custom Engines

If you are interested in adding a new engine to the project, feel free
to get in touch via Discord. The basic requirements are:

- Your engine is written for the .NET platform
- The engine passes the acceptance tests of the project
- The change does not affect the stability of the CI pipeline of the project

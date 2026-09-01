---
title: Ioxide
weight: 3
description: 'An io_uring based HTTP engine for maximum throughput on Linux.'
cascade:
  type: docs
---

{{< cards >}}
  {{< card link="https://www.nuget.org/packages/GenHTTP.Full.Ioxide/" title="GenHTTP.Full.Ioxide" icon="link" >}}
  {{< card link="https://www.nuget.org/packages/GenHTTP.Core.Ioxide/" title="GenHTTP.Core.Ioxide" icon="link" >}}
{{< /cards >}}

{{< callout type="warning" >}}
  This engine is currently a spike, not a production-ready implementation - see
  [current limitations](#current-limitations) below before adopting it.
{{< /callout >}}

Ioxide is a thread-per-reactor HTTP engine built on top of the
[ioxide](https://github.com/MDA2AV/ioxide) `io_uring` runtime. One reactor is started per
CPU core, each accepting and serving connections independently via `SO_REUSEPORT` - there is
no shared thread pool and no hand-off between threads for a given connection. Unlike the Kestrel
engine, which hands GenHTTP a fully parsed `HttpContext`, Ioxide runs GenHTTP's own HTTP/1.1
conversation directly on top of the `io_uring` pipes, the same way the Internal engine runs it on
top of sockets. According to the [HTTP Arena](https://www.http-arena.com) benchmark, this makes
it the fastest of the three engines for HTTP/1.1 (see the [performance comparison](/features/)).

```csharp
using GenHTTP.Engine.Ioxide;

await Host.Create()
          .Handler(...)
          .RunAsync();
```

## Tuning the io_uring Runtime

`Host.Create(...)` accepts an optional hook to customize the reactor count as well as ring and
buffer sizes. The hook receives a config pre-seeded with sensible defaults (one reactor per CPU
core) and should return a modified copy. The listening port always comes from the GenHTTP endpoint
binding (`.Port()`/`.Bind()`), so any port set on the config is overridden.

```csharp
using GenHTTP.Engine.Ioxide;

await Host.Create(c => c with
                  {
                      ReactorCount      = 16,
                      RingEntries       = 16384,
                      RecvBufferSize    = 64 * 1024,
                      BufferRingEntries = 8192,
                  })
          .Handler(...)
          .RunAsync();
```

`Host.Create(...)` also accepts a hook that runs once per reactor, on that reactor's own thread,
before it starts serving - useful to register per-reactor, ring-native services.

## TLS

The standard `.Bind(host, port, certificate)` overloads used by the Internal and Kestrel engines
are not wired up for Ioxide yet. TLS termination is available, but only through the lower-level
`onReactorStart`/`connectionFactory` hooks and the `ioxide.tls` package, which perform the
handshake ring-natively (OpenSSL) and then hand the connection off as kTLS:

```csharp
using GenHTTP.Engine.Ioxide;

using ioxide.tls;

await Host.Create(configure: c => c with { ExtraPorts = [8081] },
                  onReactorStart: r => IoxideTls.StartService(r, new TlsOptions
                  {
                      CertificatePath = "./cert.pem",
                      KeyPath = "./key.pem"
                  }),
                  connectionFactory: conn => conn.ListenerPort == 8081
                      ? IoxideTls.AcceptAsync(conn)
                      : new(new ConnectionDualPipe(conn)))
          .Handler(...)
          .RunAsync();
```

## Current Limitations

As of this writing, the following are not yet implemented:

- IPv6 binding and multiple endpoints via the regular `.Bind()` API (only the first configured
  endpoint is served; reaching additional ports such as a TLS listener requires the manual wiring
  shown above)
- Graceful shutdown and connection draining (reactors run as background threads; disposing the
  server stops it without waiting for in-flight connections to finish)
- The `Host` header validation and default error-response page the Internal engine provides -
  unhandled exceptions are currently swallowed rather than turned into a HTTP 500 response

## Serving Static Files

For static file serving on this engine, see the [Ioxide Engine](../../../content/handlers/files/#ioxide-engine)
section of the Files handler page - the `GenHTTP.Modules.IoxideFiles` module bakes responses
ahead of time and revalidates them via `statx` instead of assembling them per request.

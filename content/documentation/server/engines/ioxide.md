---
title: ioxide
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
  This engine is currently in preview and will be extended within the next releases of GenHTTP.
  Currently available on .NET 11 only.
{{< /callout >}}

{{< callout type="warning" >}}
  As ioxide depends on `io_uring` which is a capability of the Linux kernel, it cannot
  run on other platforms, such as Windows or macOS. Kernel version 6.1 or higher is required.
{{< /callout >}}

{{< callout type="warning" >}}
  On some system configurations, the syscalls for `io_uring` might be blocked by security
  mechanisms, so you may need to [unblock them](https://docs.docker.com/engine/security/seccomp/) explicitly.
{{< /callout >}}

An engine built on top of the [ioxide](https://github.com/MDA2AV/ioxide) `io_uring` runtime:
one reactor runs per CPU core,accepting and serving connections independently. According to the
[HTTP Arena](https://www.http-arena.com) benchmark, this engine provides the fastest webserver on the whole list.

Architecturally, this is different from the Kestrel engine, which hands GenHTTP a fully parsed
`HttpContext`: Ioxide runs GenHTTP's own HTTP/1.1 conversation directly on top of the `io_uring`
pipes, the same way the Internal engine runs it on top of sockets. There is no shared thread pool
and no hand-off between threads for a given connection.

```csharp
using GenHTTP.Engine.Ioxide;

await Host.Create()
          .Handler(...)
          .RunAsync();
```

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

## Serving `async` Payloads

ioxide achieves its performance by keeping a request on a single CPU core from
start to finish. Standard async code continues to work: Npgsql, `HttpClient`,
and any other .NET library can be used without modification. Such libraries
dispatch their work to the shared .NET thread pool, so a request may resume on
a different core than the one it started on. ioxide detects this and returns
the work to the originating core. This is correct, but not free: the cost is
incurred on every affected call and, under load, reduces the very advantage
ioxide was adopted for.

To avoid this, ioxide provides drivers that remain on the same core:
[`ioxide.pg`](https://www.nuget.org/packages/ioxide.pg/) for PostgreSQL,
[`ioxide.redis`](https://www.nuget.org/packages/ioxide.redis/) for Redis,
[`ioxide.file`](https://www.nuget.org/packages/ioxide.file/) for file access,
and a built-in HTTP client. Applications that query a database or read files
on most requests benefit the most from these drivers. In all other cases, the
standard libraries remain a suitable choice.

{{< callout type="warning" >}}
Never block inside a request handler. Calling `.Result` or `.Wait()` on a
task does not merely slow the core down, it halts it: the thread waits for
work that only it can process and will not recover until the process is
restarted. Always use `await`.
{{< /callout >}}

### Static Files

For static file serving on with GenHTTP, see the [Ioxide Engine](../../../content/handlers/files/#ioxide-engine)
section of the files handler page - the `GenHTTP.Modules.IoxideFiles` module bakes responses
ahead of time and revalidates them via `statx` instead of assembling them per request.

## Tuning the io_uring Runtime

The defaults - one reactor per CPU core, with sensible ring and buffer sizes - work for most
deployments. If you need finer control, `Host.Create(...)` accepts an optional hook that receives
a config pre-seeded with those defaults and should return a modified copy. The listening port
always comes from the GenHTTP endpoint binding (`.Port()`/`.Bind()`), so any port set on the
config is overridden.

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

## Current Limitations

As of this writing, the following are not yet implemented:

- IPv6 binding and multiple endpoints via the regular `.Bind()` API (only the first configured
  endpoint is served; reaching additional ports such as a TLS listener requires the manual wiring
  shown [below](#tls))
- Graceful shutdown and connection draining (reactors run as background threads; disposing the
  server stops it without waiting for in-flight connections to finish)
- The `Host` header validation and default error-response page the Internal engine provides -
  unhandled exceptions are currently swallowed rather than turned into a HTTP 500 response

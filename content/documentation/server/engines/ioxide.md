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

```csharp
using GenHTTP.Engine.Ioxide;

await Host.Create()
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
deployments. If you need finer control, `Host.Create(...)` takes an `EngineOptions`, grouped by the
part of the runtime each setting belongs to: the reactors (`Reactor`), the TCP transport and its TLS
(`Tcp`), the QUIC transport (`Quic`), and HTTP/3 above it (`Http3`).

```csharp
using GenHTTP.Engine.Ioxide;

await Host.Create(options: new EngineOptions
                  {
                      Reactor = new ReactorOptions
                      {
                          ReactorCount   = 16,
                          RingEntries    = 16384,
                          RecvBufferSize = 64 * 1024,
                          RecvSlots      = 8192,
                      },
                  })
          .Handler(...)
          .Bind(...)
          .RunAsync();
```

`Host.Create(...)` also takes a hook that runs once per reactor, on that reactor's own thread, before
it starts serving - useful to register per-reactor, ring-native services.

```csharp
await Host.Create(onReactorStart: reactor => ...)
          .Handler(...)
          .Bind(...)
          .RunAsync();
```

## TLS

TLS is configured through the `.Bind(...)` overload that takes a certificate provider. It is
terminated in OpenSSL on the TCP transports, and the handshake rides the same `io_uring` reads and
writes as everything else. Where a port serves both HTTP/1.1 and HTTP/2, ALPN decides which one a
connection speaks - the server offers HTTP/2 first, so a client that speaks it is served HTTP/2 and
everyone else falls back to HTTP/1.1.

```csharp
using System.Net;
using System.Security.Authentication;

using GenHTTP.Api.Infrastructure;
using GenHTTP.Engine.Ioxide;

await Host.Create()
          .Handler(...)
          .Bind(IPAddress.Any, 8443, new FileCertificateProvider("./cert.pem", "./key.pem"),
                sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls13,
                httpProtocols: HttpProtocols.Http1AndHttp2)
          .RunAsync();
```

A `FileCertificateProvider` names the certificate and its key as PEM files. Files are preferred:
OpenSSL reads the chain from the file, so intermediates come from there rather than the machine
store, and the private key never enters managed memory. On the TCP transports an already loaded
`X509Certificate2` can be passed instead; HTTP/3 cannot take one.

### Server Name Indication

If you would like to serve several host names from one port, a `HostCertificateProvider` holds one
certificate per name alongside a default:

```csharp
var certificates = new HostCertificateProvider("./localhost.pem", "./localhost.key");

certificates.Add("alpha.example", "./alpha.pem", "./alpha.key");
certificates.Add("beta.example", "./beta.pem", "./beta.key");

await Host.Create()
          .Handler(...)
          .Bind(IPAddress.Any, 8443, certificates, httpProtocols: HttpProtocols.Http1AndHttp2)
          .RunAsync();
```

The client sends the name it wants during the handshake, and the server answers with that name's
certificate. Names are matched case-insensitively and exactly - a wildcard certificate covers its
names through the certificate itself, not by being registered here. The default is not optional, as
it answers a client that sent no name or asked for one this port does not hold, which is what a bare
IP address does, since an IP is not a legal SNI value.

### HTTP/3

An HTTP/3 port always needs a certificate, and it needs it as files: ngtcp2 loads PEM by path and
takes nothing else, so an in-memory `X509Certificate2` is refused when the server starts. Bind such
a port with an `IFileCertificateProvider` - both `FileCertificateProvider` and
`HostCertificateProvider` are such. Only one endpoint may serve HTTP/3, as the engine binds a single
QUIC listener. Browsers reach HTTP/3 only after an `Alt-Svc` header points them there from a TCP
port, so a browser-facing deployment binds HTTP/3 alongside HTTP/1.1 or HTTP/2 rather than on its own.

### Client Certificates

If you would like to require a client certificate, pass an `IMutualTlsValidator` as the
`certificateValidator`, naming the anchors the client's certificate is checked against:

```csharp
public sealed class RequireClientCertificate(string clientCaPath) : IMutualTlsValidator
{
    public bool RequireCertificate => true;

    public string? ClientCaPath => clientCaPath;
}
```

```csharp
.Bind(IPAddress.Any, 8444, new FileCertificateProvider("./cert.pem", "./key.pem"),
      certificateValidator: new RequireClientCertificate("./client-ca.pem"),
      httpProtocols: HttpProtocols.Http1)
```

With `RequireCertificate` left false the connection is let in and the decision is left to the
handler. A binding that requires a certificate but names nothing to validate it against is refused
when the server starts.

### Certificate Rotation

A renewed certificate does not need a restart. `ReloadCertificates` asks each bound provider again
and installs what it answers with, across both transports, while the server keeps serving:

```csharp
using IoxideServer = GenHTTP.Engine.Ioxide.Infrastructure.Server;

var host = Host.Create()
               .Handler(...)
               .Bind(IPAddress.Any, 8443, certificates, httpProtocols: HttpProtocols.Http1AndHttp2);

// after the PEM files the providers name have been rewritten, e.g. by an ACME hook
(host.Instance as IoxideServer)?.ReloadCertificates();
```

Connections already established keep the certificate they authenticated with. Only the certificate
material changes: trust anchors, `RequireCertificate`, ALPN and the TLS floor stay as the binding
set them, and no name can be added, since both stacks settle their SNI tables at startup. Everything
is resolved and checked before anything is published, so a provider that throws, or a path an ACME
client has not finished writing, leaves the server exactly as it was.

### Kernel TLS

Kernel TLS moves the record layer into the kernel, so plaintext lands directly in ring memory. It is
off by default and is not a free win: it needs the Linux `tls` module, pins TLS 1.3 and a single
ciphersuite, and disables session resumption. `RxKernelTls` needs `TxKernelTls`, as inbound shares
the `TCP_ULP` the outbound side installs, so `Tls13` is the only floor that is consistent with it.

```csharp
await Host.Create(options: new EngineOptions
          {
              Tcp = new TcpTransportOptions
              {
                  TxKernelTls = true,
                  RxKernelTls = true,
              },
          })
          .Handler(...)
          .Bind(IPAddress.Any, 8443, new FileCertificateProvider("./cert.pem", "./key.pem"),
                sslProtocols: SslProtocols.Tls13,
                httpProtocols: HttpProtocols.Http1AndHttp2)
          .RunAsync();
```

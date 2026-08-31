---
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

If you would like to opt out of a default feature, or enable one that is off by default, you may
pass a flag as needed:

```csharp
await Host.Create()
          .Handler(...)
          .Defaults(compression: false, rangeSupport: true)
          .RunAsync();
```

`Defaults()` accepts the following flags:

| Flag              | Default | Enables                                                                     |
|-------------------|---------|------------------------------------------------------------------------------|
| `compression`     | `true`  | [Compression](../compression/) of response content.                        |
| `decompression`   | `false` | [Decompression](../decompression/) of request content.                    |
| `secureUpgrade`   | `true`  | [Automatic upgrade](../hardening/#secure-upgrade) of insecure requests.    |
| `strictTransport`  | `true`  | [HSTS](../hardening/#strict-transport) via a `Strict-Transport-Security` header. |
| `clientCaching`   | `true`  | [Client side caching](../client-caching-validation/) via `ETag` validation. |
| `rangeSupport`    | `false` | [Partial responses](../range-support/) for range requests.                 |
| `preventSniffing` | `false` | [MIME type sniffing prevention](../hardening/#prevent-sniffing) via `X-Content-Type-Options`. |

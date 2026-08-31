---
title: Hardening
description: Automatically upgrade insecure requests, send a strict transport policy and prevent MIME type sniffing.
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Security/" title="GenHTTP.Modules.Security" icon="link" >}}
{{< /cards >}}

Besides [CORS](../cors/), the security module provides a few concerns that harden a web
application against common transport-level issues. All three are added automatically by
[`Defaults()`](../defaults/) (with `preventSniffing` off), or can be added together via
`Harden()` (which also enables `preventSniffing` by default):

```csharp
await Host.Create()
          .Handler(...)
          .Harden()
          .RunAsync();
```

## Secure Upgrade

`SecureUpgrade` redirects insecure (HTTP) requests to their HTTPS equivalent. `SecureUpgrade.Force`
always redirects; `SecureUpgrade.Allow` only does so if the client requests it via the
`Upgrade-Insecure-Requests` header; `SecureUpgrade.None` disables the concern.

```csharp
using GenHTTP.Modules.Security;

await Host.Create()
          .Handler(...)
          .SecureUpgrade(SecureUpgrade.Force)
          .RunAsync();
```

## Strict Transport

`StrictTransport` sends a `Strict-Transport-Security` header, instructing browsers to only ever
contact your server via HTTPS for the given duration. `Harden()` and `Defaults()` configure it
with a 365 day policy that includes subdomains and requests preload list inclusion.

```csharp
using GenHTTP.Modules.Security;
using GenHTTP.Modules.Security.Providers;

var policy = new StrictTransportPolicy(TimeSpan.FromDays(365), includeSubdomains: true, preload: true);

await Host.Create()
          .Handler(...)
          .StrictTransport(policy)
          .RunAsync();
```

## Prevent Sniffing

`PreventSniffing` sends `X-Content-Type-Options: nosniff`, instructing browsers to trust the
`Content-Type` header of a response instead of guessing the MIME type from its content.

```csharp
using GenHTTP.Modules.Security;

await Host.Create()
          .Handler(...)
          .PreventSniffing()
          .RunAsync();
```

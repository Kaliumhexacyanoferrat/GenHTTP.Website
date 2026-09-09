---
title: Endpoints and Ports
weight: 4
description: 'Configure the GenHTTP webserver to listen on different ports or endpoints.'
cascade:
  type: docs
---

By default, the server will listen on port 8080 for incoming IPv4 or IPv6 connections on
any interface. To simply change the port the server will listen on, you can use
the `Port()` directive of the host. Please note, that hosting a server on ports below 1024
will require additional permissions on some operating systems.

```csharp
var host = Host.Create()
               .Handler(...)
               .Port(80);
```

To configure the server to listen on specific interfaces only, you can `Bind()` those
endpoints explicitly.

```csharp
var host = Host.Create()
               .Handler(...)
               .Bind(IPAddress.Parse("192.168.2.10"), 8888)
               .Bind(IPAddress.Parse("0:0:0:0:0:ffff:c0a8:20a"), 8888);
```

By default, the server will try to listen to both incoming IPv4 as well as IPv6 connections (even
if you specify an IPv4 or IPv6 address manually). To disable this behavior, specify `dualStack: false`.
The following example will listen to any incoming IPv4 request on any IP address:

```csharp
var host = Host.Create()
               .Handler(...)
               .Bind(IPAddress.Any, 8080, dualStack: false);
```

## Secure Endpoints

The `Bind()` overloads that accept a certificate or `ICertificateProvider` (see
[SSL Endpoints](../security/)) also take a `protocols` argument to restrict which TLS versions the
endpoint accepts, defaulting to TLS 1.2 and TLS 1.3:

```csharp
var host = Host.Create()
               .Handler(...)
               .Bind(IPAddress.Any, 443, myCertificate, protocols: SslProtocols.Tls13);
```

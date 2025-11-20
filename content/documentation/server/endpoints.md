---
title: Endpoints and Ports
weight: 3
description: 'Configure the GenHTTP webserver to listen on different ports or endpoints.'
cascade:
  type: docs
---

By default, the server will listen on port 8080 for incoming IPv4 or IPv6 connections on
any interface. To simply change the port the server will listen on, you can use
the `Port()` directive of the builder. Please note, that hosting a server on ports below 1024
will require additional permissions on some operating systems.

```csharp
var server = Server.Create()
                   .Handler(...)
                   .Port(80)
                   .Build();
```

To configure the server to listen on specific interfaces only, you can `Bind()` those
endpoints explicitly.

```csharp
var server = Server.Create()
                   .Handler(...)
                   .Bind(IPAddress.Parse("192.168.2.10"), 8888)
                   .Bind(IPAddress.Parse("0:0:0:0:0:ffff:c0a8:20a"), 8888)
                   .Build();
```

By default, the server will try to listen to both incoming IPv4 as well as IPv6 connections (even
if you specify an IPv4 or IPv6 address manually). To disable this behavior, specify `dualStack: false`.
The following example will listen to any incoming IPv4 request on any IP address:

```csharp
var server = Server.Create()
                   .Handler(...)
                   .Bind(IPAddress.Any, 8080, dualStack: false)
                   .Build();
```

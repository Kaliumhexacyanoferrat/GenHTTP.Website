---
title: Adding SSL Endpoints
cascade:
  type: docs
---

To add a SSL secured endpoint, you may use the overload of the `Bind` method:

```csharp
var certificate = new X509Certificate2("./mycert.pfx");

var server = Server.Create()
                   .Handler(...)
                   .Bind(IPAddress.Any, 80)
                   .Bind(IPAddress.Any, 443, certificate)
                   .Build();
```

The given certificate will be used to encrypt all incoming requests with. Note, that
the client expects the server to use a certificate with a CN matching the requested host name.

---
title: SSL Endpoints
weight: 3
description: 'Host GenHTTP via SSL/TLS endpoints and enable client certificate authentication.'
cascade:
  type: docs
---

To add a SSL/TLS secured endpoint, you can use the overload of the `Bind` method:

```csharp
var certificate = X509CertificateLoader.LoadCertificateFromFile("./mycert.pfx");

var server = Server.Create()
                   .Handler(...)
                   .Bind(IPAddress.Any, 80)
                   .Bind(IPAddress.Any, 443, certificate)
                   .Build();
```

The given certificate will be used to encrypt all incoming requests with. Please note, that
the client expects the server to use a certificate with a CN matching the requested host name.

## Dynamic Certificate Selection

If you would like to dynamically select the certificate to be used to authenticate a connection
(e.g. by the host name requested by the client), you can pass a custom `ICertificateProvider`
instance instead of a single certificate:

```csharp
public class CustomCertificateProvider : ICertificateProvider
{
    
    public X509Certificate2? Provide(string? host) 
    {
        if (host == "host1.com" || host == "www.host1.com") return ...;
        if (host == "host2.com") return ...;
        
        return null; 
    }
    
}

var server = Server.Create()
                   .Handler(...)
                   .Bind(IPAddress.Any, 80)
                   .Bind(IPAddress.Any, 443, new CustomCertificateProvider())
                   .Build();
```

## Client Certificates

To enable client certificates, you can pass a custom `ICertificateValidator` to the `Bind` method:

```csharp
public class MyValidator : ICertificateValidator
{

    public bool RequireCertificate => true;

    public X509RevocationMode RevocationCheck => X509RevocationMode.Offline;

    public bool Validate(X509Certificate? certificate, X509Chain? chain, SslPolicyErrors policyErrors)
    {
        if (policyErrors != SslPolicyErrors.None) return false;
        
        if (certificate != null)
        {
            if (certificate.Issuer == "...") return true;
        }
        
        return false;
    }

}

var server = Server.Create()
                   .Handler(...)
                   .Bind(IPAddress.Any, 80)
                   .Bind(IPAddress.Any, 443, certificate, certificateValidator: new MyValidator())
                   .Build();
```

The server will only allow clients that can present a certificate that passes the `Validate`
function of your implementation. If the client does not send a certificate, I may still connect
if `RequireCertificate` returns `false`.

Client certificates can be combined with the [client certificate authentication](/documentation/content/concerns/authentication/#client-certificate-cuthentication) concern
to authorize access on content level.

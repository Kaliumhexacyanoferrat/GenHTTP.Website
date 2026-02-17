---
title: Security
weight: 4
description: 'Security considerations and configuration options for GenHTTP applications.'
cascade:
  type: docs
---

This page summarizes security considerations and configuration options
when running GenHTTP applications.

## General Considerations

GenHTTP is designed to be used as an application server behind a reverse proxy, and therefore does not provide built-in DoS protection (such as IP connection limits or Slowloris prevention).
However, compared to many other web service frameworks for C#, it is hardened against - and explicitly tested for - typical attack vectors such as request smuggling, header injection, or malformed chunk attacks.

If you would like to run your GenHTTP application without a reverse proxy in front of it, consider using the [Kestrel-based engine](../engines/) instead.

## Encryption

To add an SSL/TLS secured endpoint, you can use the overload of the `Bind()` method:

```csharp
var certificate = X509CertificateLoader.LoadCertificateFromFile("./mycert.pfx");

var server = Server.Create()
                   .Handler(...)
                   .Bind(null, 80)
                   .Bind(null, 443, certificate)
                   .Build();
```

The given certificate will be used to encrypt all incoming requests with. Please note, that
the client expects the server to use a certificate with a CN matching the requested host name.

### Dynamic Certificate Selection

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
                   .Bind(null, 80)
                   .Bind(null, 443, new CustomCertificateProvider())
                   .Build();
```

### Client Certificates

To enable client certificates (mTLS), you can pass a custom `ICertificateValidator` to the `Bind` method:

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
                   .Bind(null, 80)
                   .Bind(null, 443, certificate, certificateValidator: new MyValidator())
                   .Build();
```

The server will only allow clients that can present a certificate that passes the `Validate`
function of your implementation. If the client does not send a certificate, I may still connect
if `RequireCertificate` returns `false`.

Client certificates can be combined with the [client certificate authentication](/documentation/content/concerns/authentication/#client-certificate-cuthentication) concern
to authorize access on content level.

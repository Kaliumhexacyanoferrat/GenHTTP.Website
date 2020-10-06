## Redirects

The redirect provider allows to inform the client that the location of the actual resource
is different than the requested URI.

```csharp
layout.Add("redirect", Redirect.To("http://google.com"))
```

In this example, accessing http://localhost:8080/redirect will redirect the client to the
Google search engine. By default, the HTTP status 301 (permament redirect) will be sent, which
can get customized to a temporary HTTP 307 redirect.
## Downloads

Downloads allow to serve a single file with a fixed name to your clients. The content type
of the file will automatically be determined by it's extension.

```csharp
layout.Add("agb.pdf", Download.FromFile("/var/www/documents/agb.pdf"))
```

In this example, the file would be available at http://localhost:8080/agb.pdf.
As with static content, downloads can also be served from your embedded resources.
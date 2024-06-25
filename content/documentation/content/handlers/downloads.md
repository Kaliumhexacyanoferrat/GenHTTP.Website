---
title: Downloads
description: 'Provide files via HTTP to requesting clients as a download, e.g. from the file system.'
cascade:
  type: docs
---

Downloads allow to serve a single [Resource](../../concepts/resources) with a fixed name to your clients. The content type
of the file will automatically be determined by it's extension.

```csharp
var resource = Resource.FromFile("/var/www/documents/agb.pdf");

layout.Add("agb.pdf", Download.From(resource))
```

In this example, the file would be available at http://localhost:8080/agb.pdf.

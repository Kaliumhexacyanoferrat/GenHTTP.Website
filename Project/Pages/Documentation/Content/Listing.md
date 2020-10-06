## Directory Browsing

The directory listing provider serves a simple web UI allowing users to browse directories
and files below the specified path.

```csharp
Host.Create()
    .Handler(DirectoryListing.From("/var/www/documents/"))
    .Run();
```

In this example, the listing view will be available at http://localhost:8080/.
The generated view will be rendered using the template defined for the section
of the website the provider is embedded into.
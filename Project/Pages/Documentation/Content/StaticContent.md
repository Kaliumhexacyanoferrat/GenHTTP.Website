## Static Resources

To provide static resources required by your web application, add them to your project
and mark them either as `Content` or as an `Embedded Resource`. To serve those files,
you can use the `Resources` factory class:

```csharp
var layout = Layout.Create();

// serve all embedded resources in the "Resources" sub folder of your project
var tree = ResourceTree.FromAssembly("Resources");

// serve all files in the given folder
var tree = ResourceTree.FromDirectory("./Resources");

layout.Add("res", Resources.From(tree));

Host.Create()
    .Handler(layout)
    .Run();
```

For example, a stylesheet named `main.css` in the `styles` subfolder would be made available at
http://localhost:8080/res/styles/main.css. To generate a relative link to the file, use the routing
functionality from within your template:

```html
{{ route 'res/styles/main.css' }}
```
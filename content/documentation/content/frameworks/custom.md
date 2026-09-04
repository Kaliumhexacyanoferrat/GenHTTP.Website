---
title: Custom Frameworks
description: Shows how to use the underlying capabilities of GenHTTP to spin of a custom framework.
weight: 7
cascade:
  type: docs
---

The framework layer of GenHTTP allows application frameworks (such as webservices or controllers) to host a set
of operations (put simply a route with a piece of C# code to be executed). This way the application framework
itself is only a very thin layer - it analyzes the given scope (such as a controller class) and derives operations
from it which are then handled by the generic framework part. This way, all application frameworks share
the same feature level and can support things like parameter binding, serialization, async execution
or code generation. To give an example, the webservice handler which provides the whole webservice framework
consists of 70 lines of code only - everything else is done by the underlying framework layer.

In essence, the framework layer allows you to bind a dynamic bunch of C# methods to HTTP endpoints with some
granularity to configure how they are exposed. Examples could be a framework mapping database access to 
REST or a framework to emulate file-based endpoints as we have with PHP.

{{< callout type="warning" >}}
  The reflection module is currently considered an internal dependency of the webservice frameworks, so we do
  not guarantee SemVer as we do with the public API surface of GenHTTP.
{{< /callout >}}

## Implementing a Custom Framework

To give an example, we will create a simple handler that creates some operations and delegates their execution
to a `MethodCollection` handler.

```csharp
public class CustomFrameworkHandler : IHandler, IServiceMethodProvider
{
    private MethodCollection? _methods;

    public MethodCollection Methods => _methods ?? throw new InvalidOperationException("Handler is not prepared yet");

    public async ValueTask PrepareAsync(IServer server)
    {
        // this snippet exposes a given method as an operation.
        // your framework will typically analyze a scope given by the
        // user to dynamically collect a bunch of operations
        
        var list = new List<MethodHandler>();

        // specify the supported methods of the operation we are exposing
        var supportedMethods = new MethodConfiguration([RequestMethod.Get]);

        // the actual piece of code to be executed - either a method info or a delegate
        var methodInfo = GetType().GetMethod("ExposedMethod")!;

        // auto enables code generation, otherwise reflection only
        var executionSettings = new ExecutionSettings(ExecutionMode.Auto);

        // configures the behavior for serialization, injection and formatting
        var registry = new MethodRegistry(
            Serialization.Default().Build(),
            Injection.Default().Build(),
            Formatting.Default().Build()
        );

        // build the operation we would like to provide
        var operation = OperationBuilder.Create(server, "/path/:id", methodInfo, null, executionSettings, supportedMethods, registry);

        // create a method handler from the operation which will serve it as an HTTP endpoint
        // this handler requires an instance provider which tells the framework on which
        // object to invoke the given method info or delegate
        list.Add(new MethodHandler(operation, (_) => new(this), registry));

        // build a method collection handler from all collected operations
        // this handler is responsible for routing
        _methods = new MethodCollection(list);

        await _methods.PrepareAsync(server);
    }

    public ValueTask<IResponse?> HandleAsync(IRequest request) => Methods.HandleAsync(request);

    public string ExposedMethod(int id) => id.ToString();

}
```

Hosting this handler will expose `/path/:id` on our server, where `:id` is automatically mapped as a path
argument and converted into an integer. The given operation will be compiled using code generation
in the preparation phase of the `MethodCollection`.

By implementing `IServiceMethodProvider`, our custom framework seamlessly integrates into
functionality that discovers available operations, such as the Open API handlers.

---
title: Dependency Injection
description: Enable dependency injection on the GenHTTP server.
weight: 6
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.DependencyInjection/" title="GenHTTP.Modules.DependencyInjection" icon="link" >}}
{{< /cards >}}

This optional module allows to use Microsoft's abstraction layer to inject dependencies into handlers, 
concerns and framework classes as well as their methods.

## Enabling Dependency Injection

After adding the nuget package, you can globally enable dependency injection in your application by calling 
the `AddDependencyInjection` method on the host builder, passing a pre-configured service provider
to be used. As expected, the lifecycle of the dependencies is managed by the service provider,
so you can use singletons, scoped or transient services as needed. The implementation will create
a new scope for each request handled by the server.

```csharp
using GenHTTP.Engine.Internal;

using GenHTTP.Modules.DependencyInjection;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Practices;

using Microsoft.Extensions.DependencyInjection;

var app = Layout.Create();

// create a service collection and register your dependencies
var services = new ServiceCollection();

services.AddSingleton<FirstDependency>()
        .AddSingleton<SecondDependency>();

var provider = services.BuildServiceProvider();

await Host.Create()
          .AddDependencyInjection(provider)
          .Handler(app)
          .Defaults()
          .Development()
          .Console()
          .RunAsync();
```

## Framework Integration

The following sections show how you can add dependency injection to webservices and websockets.

### Service Frameworks

The dependency injection module adds extensions to the layout builder that can be used to add DI-enabled
web services, controllers, or functional handlers to your application. This requires dependency injection
to be configured as described above. The following examples show how to inject the types registered
above into services or controllers.

{{< tabs items="Webservices,Functional,Controllers" >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Modules.DependencyInjection;
  
  var app = Layout.Create()
                  .AddDependentService<InjectionDemo>("service");
  
  public class InjectionDemo
  {
      private readonly FirstDependency _first;
      
      public InjectionDemo(FirstDependency first) 
      {
          _first = first;
      }
      
      [ResourceMethod]
      public string DoSomething(SecondDependency second) => "42";
      
  }
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Modules.DependencyInjection;
  
  var app = DependentInline.Create()
                           .Get((FirstDependency first) => "42");
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Modules.DependencyInjection;
  
  var app = Layout.Create()
                  .AddDependentController<InjectionDemo>("controller");
  
  public class InjectionDemo
  {
      private readonly FirstDependency _first;
      
      public InjectionDemo(FirstDependency first) 
      {
          _first = first;
      }
      
      public string DoSomething(SecondDependency second) => "42";
      
  }
  ```
{{< /tab >}}

{{< /tabs >}}

In contrast to non-injected services, the framework will create a new instance of your service
on every request so that scoped dependencies can be injected. To control the lifecycle of your
services, you can also register them in the service provider, and they will be resolved from there.

### Websockets

The dependency injection module adds extension methods that allow dependent websocket handlers to be
created with having their dependencies injected as expected.

{{< tabs items="Reactive,Imperative" >}}

{{< tab >}}
  ```csharp
  var websocket = Websocket.Reactive() 
                           .DependentHandler<MyHandler>();
  
  class MyHandler(MyDependency dependency) : IReactiveHandler
  {
  
      public ValueTask OnMessage(IReactiveConnection connection, IWebsocketFrame message)
      {
          // do work
      }
  
  }
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  var websocket = Websocket.Imperative()
              .DependentHandler<MyHandler>();
  
  class MyHandler(MyDependency dependency) : IImperativeHandler
  {
  
      public ValueTask HandleAsync(IImperativeConnection connection)
      {
          // do work
      }
  
  }
  ```
{{< /tab >}}

{{< /tabs >}}

## Handler and Concern Integration

The `Dependent` entry point allows you to create handlers and concerns that resolve their
dependencies from the registered service provider. As their lifecycle may differ from that
of regular handlers and concerns, there are slightly different interfaces to be implemented.

{{< tabs items="Handlers,Concerns" >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Modules.DependencyInjection;
  
  var app = Layout.Create()
                  .Add("handler", Dependent.Handler<InjectionHandler>());
  
  public class InjectionHandler : IDependentHandler
  {
      private readonly FirstDependency _first;
      
      public InjectionHandler(FirstDependency first) 
      {
          _first = first;
      }
      
      public ValueTask<IResponse?> HandleAsync(IRequest request) 
      {
          // ...
      }
      
  }
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Modules.DependencyInjection;
  
  var app = Layout.Create()
                  .Add(Dependent.Concern<InjectionConcern>());
  
  public class InjectionConcern : IDependentConcern
  {
      private readonly FirstDependency _first;
      
      public InjectionConcern(FirstDependency first) 
      {
          _first = first;
      }
      
      public ValueTask<IResponse?> HandleAsync(IHandler content, IRequest request) 
      {
          return content.HandleAsync(request);
      }
      
  }
  ```
{{< /tab >}}

{{< /tabs >}}

## Accessing Scope and Service Provider

To obtain the currently used service provider or scope, there are extension methods available
on the `IRequest` interface:

```csharp
using GenHTTP.Modules.DependencyInjection;

var serviceProvider = request.GetServiceProvider();
var serviceScope = request.GetServiceScope();
```

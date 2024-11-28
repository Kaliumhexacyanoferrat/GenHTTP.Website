---
title: Controllers
description: Lightweight framework to write web APIs using the controller pattern in C#.
weight: 3
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Controllers/" title="GenHTTP.Modules.Controllers" icon="link" >}}
{{< /cards >}}

This framework allows to create APIs by defining and implementing controller classes. In contrast
to ASP.NET you cannot use this framework to dynamically render HTML pages in a MVC style project,
so this is just another flavor of defining a web API.

{{< callout type="info" >}}
Controller based APIs can quickly be created by using a [project template](../../templates/).
{{< /callout >}}

## Hosting an API

The following example shows how an API controlling an IoT device could look like
when implemented using the controller framework.

The methods of the controller class will automatically be translated into paths
that can be called by the client.

```csharp
// API will be available via http://localhost:8080/device/

var api = Layout.Create()
                .AddController<IotController>("device")
                .Add(CorsPolicy.Permissive());
                
await Host.Create()
          .Handler(api)
          .Defaults()
          .Development()
          .Console()
          .RunAsync();
    
// --

public class IotController
{

    public DeviceInfo Index() 
    {
        // GET http://localhost:8080/device/
    }
    
    public DeviceFieldInfo Field([FromPath] int fieldID) 
    {
        // GET http://localhost:8080/device/field/4711
    }
    
    [ControllerAction(RequestMethod.POST)]
    public DeviceInfo Restart() 
    {
        // POST http://localhost:8080/device/restart
    }

}
```

## URL Patterns

The following table shows the rules that are applied to map an URL:

| Method                      | Endpoint      |
| -------------               | ------------- |
| `Index()`                   | `/controller/` |
| `Action()`                  | `/controller/action/` |
| `Action([FromPath] int id)` | `/controller/action/:id/` |
| `Action(int id)`            | `/controller/action/?id=:id` |
| `LongAction()`              | `/controller/long-action/` |

## Further Resources

The following capabilities are shared by various application frameworks:

{{< cards >}}
{{< card link="../../concepts/definitions" title="Method Definitions" icon="chip" >}}
{{< /cards >}}

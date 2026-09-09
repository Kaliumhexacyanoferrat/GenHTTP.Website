---
title: Logging
weight: 3
description: 'Describes how to configure GenHTTP for logging.'
cascade:
  type: docs
---

GenHTTP uses the [default logging mechanism](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/overview)
of .NET Core and allows to create loggers via the `IServer` interface. By default,
the host will add a console logger.

## Logging Information

The following code shows to obtain a logger from the server instance in
a custom `IHandler` implementation. It is recommended to create the logger
once in the preparation phase and store the reference somewhere. You can use
this pattern anywhere where you have access to `IServer` (e.g. via `request.Server`).

```csharp
using GenHTTP.Api.Content;
using GenHTTP.Api.Infrastructure;
using GenHTTP.Api.Protocol;

using Microsoft.Extensions.Logging;

namespace GenHTTP.Playground;

public class MyHandler : IHandler
{
    private ILogger? _logger;

    public ValueTask PrepareAsync(IServer server)
    {
        _logger = server.Logging.CreateLogger<MyHandler>();
        return ValueTask.CompletedTask;
    }

    public ValueTask<IResponse?> HandleAsync(IRequest request)
    {
        _logger?.LogInformation("I am logging important information");   
        
        return new();
    }
    
}
```

## Customize Logging

The `Logging()` method of the host builder allows to pass a custom
logging factory that should be used by the application. 

The following example uses Open Telemetry for logging:

```csharp
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;

var factory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(logging =>
    {
        logging.AddOtlpExporter();
    });
});

return await Host.Create()
                 .Logging(factory)
                 .Handler(handler)
                 .Defaults()
                 .RunAsync();
```

To disable logging, you can pass a null logger:

```csharp
Host.Create()
    .Logging(NullLoggerFactory.Instance);
```

## Request Logging

By default, GenHTTP will install a concern that logs every request and the
response it generated.

```bash
info: GenHTTP.Requests[217529272]
      GET /api/ - HTTP 200 - 12 bytes - 18521.44 ms
info: GenHTTP.Requests[217529272]
      GET /favicon.ico - HTTP 404 - 0 bytes - 149.87 ms
info: GenHTTP.Requests[217529272]
      GET /assets/GenHTTP.Playground.xml - HTTP 200 - 0 bytes - 103.06 ms
```

If you would like to opt-out of request logging, you can pass a flag to the
`Logging()` method of the host builder.

```csharp
var logger = LoggerFactory.Create(b => b.AddConsole());

return await Host.Create()
                 .Logging(logger, logRequests: false)
                 .Handler(handler)
                 .Defaults()
                 .RunAsync();
```

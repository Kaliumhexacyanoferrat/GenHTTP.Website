---
title: Logging
weight: 5
description: 'Configure how the GenHTTP server logs status changes and handled requests.'
cascade:
  type: docs
---

The server logs to the console by default, using the standard `Microsoft.Extensions.Logging`
abstraction, and installs a concern that logs every handled request. Both are configured via
`Logging()` on the host:

```csharp
using Microsoft.Extensions.Logging;

var host = Host.Create()
               .Handler(...)
               .Logging(LoggerFactory.Create(builder => builder.AddConsole()));
```

Pass `logRequests: false` if you want to keep the logger factory but not have every request logged:

```csharp
var host = Host.Create()
               .Handler(...)
               .Logging(myLoggerFactory, logRequests: false);
```

To disable logging entirely, pass a `NullLoggerFactory`:

```csharp
using Microsoft.Extensions.Logging.Abstractions;

var host = Host.Create()
               .Handler(...)
               .Logging(NullLoggerFactory.Instance);
```

Because `ILoggerFactory` is the standard .NET abstraction, you can plug in any logging provider
(e.g. Serilog or Application Insights) the same way you would in an ASP.NET Core application.

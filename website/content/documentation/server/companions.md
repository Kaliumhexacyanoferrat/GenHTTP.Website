---
title: Companions
cascade:
  type: docs
---
As the server is designed to require as few dependencies as possible, it will neither create log files
nor log errors to the console. You can register a [Companion](https://github.com/Kaliumhexacyanoferrat/GenHTTP/blob/master/API/Infrastructure/IServerCompanion.cs)
instance that allows you to handle errors and log requests:

```csharp
public class ConsoleCompanion : IServerCompanion
{
 
    public void OnRequestHandled(IRequest request, IResponse response)
    {
        Console.Log(request.Path);
    }

    public void OnServerError(ServerErrorScope scope, IPAddress? client, Exception error)
    {
        Console.Log(error);
    }

}
```

If you are fine with a simple console-based logger, chain the following call to your server setup:

```csharp
var server = Server.Create().Handler(...).Console();
```

## Performance

As companion methods are called synchronously on the thread the request is handled in,
you should not perform heavy, long-running tasks in those methods.

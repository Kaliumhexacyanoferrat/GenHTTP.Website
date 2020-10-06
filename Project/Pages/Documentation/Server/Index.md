## Lifecycle

As a basic requirement, the server needs a handler to be supplied
to serve the incoming requests:

```csharp
var layout = Layout.Create();

var server = Server.Create()
                   .Defaults()
                   .Handler(layout);
                        
using (var instance = server.Build())
{
    Console.ReadLine();                        
}
```

The server will start to listen for requests as soon as the `Build`
method is called. When disposed, the server will stop to process messages
and release all claimed resources.

## Basic Infrastructure

The default server instance is configured to provide basic features that are
usually required by every web application or service. If you would like to
customize those features, see the following sections:

- [Endpoints and Ports](./endpoints)
- [Compression](./compression)

## Security

By default, the server will provide an HTTP endpoint on port 8080 with no
SSL supported enabled. It's recommended to serve all of your web applications
by a dedicated reverse proxy such as [nginx](https://www.nginx.com/)
or the [GenHTTP Gateway](https://hub.docker.com/r/genhttp/gateway).
Nevertheless, the server allows you to add HTTPS endpoints to your application.

- [Secure Endpoints](./security)

## Extensibility

To add additional behavior to your web application or service, you can register
additional elements with your server.

- [Companions](./companions)
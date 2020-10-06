## Lifecycle

As a basic requirement, the server always needs a handler to be supplied
to serve incoming requests. See the [Providing Content](/documentation/content/) section
to get a list of suitable handlers.

Server instances are usually created using the `Host` factory. Depending on your
use case, you can either use a blocking or non-blocking call.

```csharp
// the content we would like to serve
var content = Content.From("Hello World!");

// blocking until the application receives a shutdown signal (e.g. main method of a standalone application)
return Host.Create()
           .Handler(content)
           .Run();

// non-blocking (e.g. for test libraries)
var host = Host.Create()
               .Handler(content)
               .Start();

try {
    // do something with the server instance
}
finally {
    // release resources
    host.Stop();
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
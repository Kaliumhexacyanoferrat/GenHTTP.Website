## Basic Concepts

Starting a GenHTTP server instance will always require you to specify the handler
that is responsible to generate responses to client requests. Depending on the kind
of content you would like to serve (such as a website or a webservice), there are various
handlers already available to be used.

```csharp
Host.Create().Handler(...).Run();
```

The structure of your web application can be defined using a layout handler
which allows you to define routes and sections.

```csharp
var shop = Layout.Create()
                 .Add("checkout", Page.From(...));

var api = Layout.Create()
                .Add<CartResource>("cart");

var project = Layout.Create()
                    .Index(Page.From(...))
                    .Add("shop", shop) // e.g. http://localhost:8080/shop/checkout
                    .Add("api", api); // e.g. http://localhost:8080/api/cart/items

var website = Website.Create()
                     .Content(project);
```

By adding concerns to your handlers you can define additional behavior for
different sections of your app such as logging, authentication, or caching.

```csharp
website.Compression(CompressedContent.Default());
```

The following sections describe the handlers that are shipped with the
GenHTTP framework.

## Application Frameworks

- [Websites](./websites)<br />
  Allows you to serve a themed web application with basic features such as
  templating, theming, sitemaps, or robots instruction files.

- [Webservices](./webservices)<br />
  Provides REST based web services that can be consumed by clients to
  retrieve a JSON or XML serialized result.

- [Single Page Applications (SPA)](./single-page-applications)<br />
  Allows to serve applications written with modern JS frameworks such as
  Vue.js, Angular or React.

## Concerns

- [Authentication](./authentication)<br />
  Restricts the content provided by a section to authenticated users.

## Content Providers

- [Static Content](./static-content)<br />
  Serves resources stored in a directory or as embedded resources within an
  assembly to the client.

- [Downloads](./downloads)<br />
  Provides a file to the client as a download. This could be a file from
  the file system or from somewhere else.

- [Redirects](./redirects)<br />
  Redirects the client to another location on the server or an
  external site.

## Infrastructure

- [Directory Browsing](./listing)<br />
  Provides a simple, recursive web view on a directory on the file system.

- [Reverse Proxies](./reverse-proxies)<br />
  Relays the request of the client to another server and returns
  the response fetched from there.

- [Virtual Hosts](./virtual-hosts)<br />
  Allows you to handle requests differently depending on the host specified
  by the client. This allows to serve multiple domains using a single
  server instance.

- [Load Balancer](./load-balancing)<br />
  Randomly distributes incoming requests to specified nodes to distribute the 
  resulting load to other systems.
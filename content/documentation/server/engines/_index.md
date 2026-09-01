---
title: Engines
weight: 1
description: 'Describes the webserver engines available to run GenHTTP projects with (such as Kestrel or Ioxide).'
cascade:
  type: docs
---

GenHTTP is both a HTTP server implementation and a web service application framework.
Depending on your requirements, the underlying HTTP engine can be replaced with another web server.

The acceptance tests of the project ensure that you can replace the engine without any further
adjustments to your application code - so basically all you do is changing the core nuget package
and the `Host` namespace you import.

## Choosing an Engine

| Engine                  | Package                | Good fit for                                                                  |
|-------------------------|------------------------|-------------------------------------------------------------------------------|
| [Internal](./internal/) | `GenHTTP.Core`         | Embedding into another application, small Docker containers, few dependencies |
| [Kestrel](./kestrel/)   | `GenHTTP.Core.Kestrel` | Edge servers, security requirements, HTTP/2 and HTTP/3                        |
| [Ioxide](./ioxide/)     | `GenHTTP.Core.Ioxide`  | Maximum HTTP/1.1 throughput on Linux; currently a spike, not production ready |

## Requirements

| Engine   | Target Frameworks | Platform                                   |
|----------|-------------------|--------------------------------------------|
| Internal | .NET 10, .NET 11  | any                                        |
| Kestrel  | .NET 10, .NET 11  | any                                        |
| Ioxide   | .NET 11 only      | relies on `io_uring`, so effectively Linux |

## Custom Engines

If you are interested in adding a new engine to the project, feel free
to get in touch via Discord. The basic requirements are:

- Your engine is written for the .NET platform
- The engine passes the acceptance tests of the project
- The change does not affect the stability of the CI pipeline of the project

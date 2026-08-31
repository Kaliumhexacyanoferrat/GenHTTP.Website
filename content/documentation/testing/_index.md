---
title: Testing
weight: 3
description: 'Introduction to testing applications written by using the GenHTTP framework.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Testing/" title="GenHTTP.Testing" icon="link" >}}
{{< /cards >}}

The `GenHTTP.Testing` package provides an easy way to write component tests for
your application using a test framework of your choice. It provides both the
ability to host your project in an isolated mode as well as convenience methods
to run HTTP requests against your server.

## Writing Tests

The following code shows how the `TestHost` can be used to spin up a server instance
hosting the functionality of the app to be tested and how to run requests against
this instance.

```csharp
using GenHTTP.Testing;

[TestClass]
public sealed class MyTests
{

    [TestMethod]
    public async Task TestMyApp()
    {
        var app = ... // setup your app here

        await using var runner = await TestHost.RunAsync(app);

        using var response = await runner.GetResponseAsync("/some/path");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

}
```

The framework uses the `HttpClient` to execute requests, so that the semantics
are the same, e.g. when performing POST requests with a body:

```csharp
var request = runner.GetRequest();

request.Method = HttpMethod.Post;
request.Content = new StringContent("My Body");

using var response = await runner.GetResponseAsync(request);
```

## Response Handling

The test framework provides some extension methods to simplify reading typed responses.

```csharp
using var response = await runner.GetResponseAsync();

var typed = await response.GetContentAsync<MyType>();

var typedNullable = await response.GetOptionalContentAsync<MyType>(); // might be null
```

Those methods allows to deserialize all formats supported by the GenHTTP framework
(JSON, XML, YAML, form encoded, Protobuf).

```csharp
var header = response.GetHeader("X-My-Header");
var contentType = response.GetContentHeader("Content-Type");
```

## Testing Against Every Engine

`RunAsync` accepts a `TestEngine` (`Internal` by default, or `Kestrel`/`Ioxide`), so the same test
suite can be run against every [engine](../../server/engines/) without changing a single assertion -
useful to catch engine-specific regressions early.

```csharp
[TestMethod]
[DataRow(TestEngine.Internal)]
[DataRow(TestEngine.Kestrel)]
[DataRow(TestEngine.Ioxide)]
public async Task TestMyApp(TestEngine engine)
{
    var app = ... // setup your app here

    await using var runner = await TestHost.RunAsync(app, engine: engine);

    using var response = await runner.GetResponseAsync("/some/path");

    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
}
```

`RunAsync` (and the `TestHost` constructor) also accept `defaults` and `development`, both `true`
by default, controlling whether `.Defaults()` and `.Development()` are applied to the hosted server.

## Configuring the HTTP Client

`TestHost.GetClient(...)` builds an `HttpClient` preconfigured for testing (no proxy, a 15 second
timeout) and lets you opt into the behavior a plain `HttpClient` doesn't have by default - useful
when testing redirects, authentication, cookies or HTTP/2:

```csharp
var client = TestHost.GetClient(ignoreSecurityErrors: true, followRedirects: true,
                                 protocolVersion: HttpVersion.Version20,
                                 creds: new NetworkCredential("user", "pass"),
                                 cookies: new CookieContainer());

using var response = await runner.GetResponseAsync("/some/path", client);
```

## Accessing the Live Server

If a test needs the actual URL the server is listening on (e.g. to hand it to another library),
use `GetUrl()`. `TestHost.NextPort()` reserves the next free port used by the test infrastructure,
in case you need to bind additional resources alongside the server under test.

```csharp
var url = runner.GetUrl("/some/path");
```

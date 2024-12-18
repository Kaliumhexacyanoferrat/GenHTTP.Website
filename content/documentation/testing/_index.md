﻿---
title: Testing
weight: 3
description: 'Introduction to testing applications written by using the GenHTTP framework.'
cascade:
  type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Testing/" title="GenHTTP.Modules.Testing" icon="link" >}}
{{< /cards >}}

The `GenHTTP.Testing` package provides an easy way to write component tests for
your application using a test framework of your choice. It provides both the
ability to host your project in an isolated mode as well as convenience methods
to run HTTP requests against your server.

{{< callout type="info" >}}
Projects created via [project templates](../content/templates/) already feature a basic test setup.
{{< /callout >}}

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

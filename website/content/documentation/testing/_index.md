---
title: Testing
cascade:
  type: docs
---

The `GenHTTP.Testing` package provides an easy way to write component tests for
your application using a test framework of your choice. It provides both the
ability to host your project in an isolated mode as well as convenience methods
to run HTTP requests against your server.

> <span class="note">NOTE</span> Projects created via [project templates](../content/templates) already feature a basic test setup.

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

        using var runner = TestHost.Run(app);

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

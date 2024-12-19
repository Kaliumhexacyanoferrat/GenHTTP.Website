---
title: Authentication
description: 'Restrict the content provided by the server to authenticated users.'
cascade:
type: docs
---

{{< cards >}}
{{< card link="https://www.nuget.org/packages/GenHTTP.Modules.Authentication/" title="GenHTTP.Modules.Authentication" icon="link" >}}
{{< /cards >}}

The authentication module allows to restrict access to sections provided 
by handlers to authenticated users only. Independent from the kind of authentication
used, the identified user can be read from the request in the following manner:

```csharp
using GenHTTP.Modules.Authentication;
                        
var displayName = request.GetUser<IUser>()?.DisplayName;
```

The user object for the current request will be determined by the installed authentication
concern. The concern will allow you to add your custom user object there.

For frameworks such as webservices or controllers, you can also inject the user instance in
your method calls (see [user injection](/documentation/content/concepts/definitions/#user-injection)) or use [role-based authorization](#role-based-authorization).

## API Key Authentication

This kind of authentication requires your clients to authenticate themselves using an key sent
with an additional HTTP header (named `X-API-Key`). 

```csharp
var securedContent = Layout.Create();

var auth = ApiKeyAuthentication.Create()
                               .Keys("ABC-123", "BCD-234");

securedContent.Add(auth);
```

You may customize both where the key is read from and what keys are valid for authentication:

```csharp
var auth = ApiKeyAuthentication.Create()
                               .WithQueryParameter("key") // read the key from the query ..
                               .WithHeader("key") // .. or a custom header ...
                               .Extractor((r) => ...) // ... or anywhere else
                               .Authenticator(DoAuthenticateAsync);

private ValueTask<IUser?> DoAuthenticateAsync(IRequest request, string key)
{
    // do something to check the key (e.g. query a database)
    return new(new ApiKeyUser(key)); // or null, if no access is granted
}
```

## Bearer Authentication

Bearer authentication expects the clients to send an access token that they obtained
from a trusted issuer with every request. The concern will validate the tokens
against the signing keys of the issuer and will optionally perform additional
validation steps to authorize the client.

```csharp
var securedContent = Layout.Create();

// specify all users and their password ...
var auth = BearerAuthentication.Create()
                               .Issuer("https://sso.mycompany.org/auth/")
                               .Audience("https://myapi.company.org"); // optional

securedContent.Add(auth);
```

Please note, that both the issuer and the audience url need to exactly match the
issued values (including trailing slashes etc.). To validate the issuer, the concern
needs to download the signing keys from the server on first request. Subsequent requests
will use the cached keys.

If you would like to perform additional checks, you can pass a custom validator:

```csharp
var auth = BearerAuthentication.Create()
                               .Issuer(...)
                               .Validation(ValidateClient);
                               
// ...

private static Task ValidateClient(JwtSecurityToken token)
{
    if (token.Subject != "my-client")
    {
        throw new ProviderException(ResponseStatus.Forbidden, "Only 'my-client' is allowed to access this service");
    }

    return Task.CompletedTask;
}
```

If you would like to map incoming tokens to user accounts, you can add a mapper which will
resolve an `IUser` instance. This mechanism can be combined with [user injection](/documentation/content/concepts/definitions/#user-injection)
to allow you to directly access the user instance in your service methods.

```csharp
var auth = BearerAuthentication.Create()
                               .UserMapping(MapUser)
                               .AllowExpired();
                               
// ...

private static ValueTask<IUser?> UserMapping(IRequest request, JwtSecurityToken token)
{
    var user = UserRepository.Resolve(token.Subject);

    return new ServiceUser(user);
}

// ...

[ResourceMethod]
public MyResponse DoWork(ServiceUser user) 
{
  // ...
}
```

Returning `null` will not deny access for the requesting client. If you want to do so,
throw an `ProviderException` with status `Forbidden`.

## Client Certificate Authentication

This concern can be used to secure a section of your web application by analyzing the 
certificate the client presented during the SSL/TLS handshake when connecting
to the server. The configuration for this feature can be found [here](/documentation/server/security/#client-certificates).

```csharp
var securedContent = Layout.Create();

var auth = ClientCertificateAuthentication.Create()
                                          .Authorization(Authorize)
                                          .UserMapping(MapUser);

securedContent.Add(auth);

//

static ValueTask<bool> Authorize(IRequest request, X509Certificate? certificate)
{
    // return true to allow the request, false to deny it
    return new(certificate != null);
}

static ValueTask<IUser?> MapUser(IRequest request, X509Certificate? certificate)
{
    // create the user record you would like to use for this request
    if (certificate != null)
    {
        return new(new ClientCertificateUser(certificate));
    }

    return new();
}
```

## Basic Authentication

Basic authentication can be used by either specifying a list of users and their passwords or
by implementing your custom logic to authenticate and load users (e.g. from a database).

```csharp
var securedContent = Layout.Create();

// specify all users and their password ...
var auth = BasicAuthentication.Create()
                              .Add("Bob", "pw123")
                              .Add("Alice", "123pw");

securedContent.Add(auth);

// ... or implement your custom logic
securedContent.Authentication((user, password) => {
   // validate the given credentials here and return your custom user object which needs to implement IUser
   return new(new BasicAuthenticationUser(user));
});
```

## Mixed Authentication

The `MultiAuthentication` concern allows you to use multiple authentication methods
at the same time, so for example you can authenticate your API both with API keys or
basic authentication. The concern will try the authentication mechanisms in the order
you added them to the builder.

```csharp
var keyAuth = ApiKeyAuthentication.Create()
                                  .Keys("ABC-123");

var basicAuth = BasicAuthentication.Create()
                                   .Add("Bob", "pw123");

var mixedAuth = MultiAuthentication.Create()
                                   .Add(keyAuth)
                                   .Add(basicAuth);

var api = Layout.Create()
                .AddService<...>()
                .Add(mixedAuth);
```

Please note that if you would like to support basic authentication via browser, you need
to add the `BasicAuthentication` last, as the response generated by this concern will contain
the challenge that instructs the browser to ask the user for credentials.

## Role-based Authorization

If you are using one of the service frameworks, you probably want to authorize users on method level
(e.g. an admin is allowed to delete something whereas regular users are not). This can be achieved
by setting the `Roles` property on user creation in combination with the `[RequireRole]`
interceptor:

{{< tabs items="Webservices,Functional,Controllers" >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Api.Content.Authentication;
  using GenHTTP.Api.Protocol;

  using GenHTTP.Engine.Internal;

  using GenHTTP.Modules.Authentication;
  using GenHTTP.Modules.Authentication.ApiKey;
  using GenHTTP.Modules.Layouting;
  using GenHTTP.Modules.Webservices;

  var auth = ApiKeyAuthentication.Create()
                                 .WithQueryParameter("apiKey")
                                 .Authenticator(AuthenticateRequestAsync);

  var app = Layout.Create()
                  .AddService<MyService>("service")
                  .Authentication(auth);

  await Host.Create()
            .Handler(app)
            .RunAsync();

  static ValueTask<IUser?> AuthenticateRequestAsync(IRequest request, string apiKey)
  {
      if (apiKey == "abc")
      {
          return new(new ApiKeyUser(apiKey, "ADMIN", "USER"));
      }

      if (apiKey == "bcd")
      {
          return new(new ApiKeyUser(apiKey, "USER"));
      }

      return default;
  }

  public class MyService
  {

      [ResourceMethod("/admin")]
      [RequireRole("ADMIN")]
      public string CheckAdmin() => "You are an administrator";

      [ResourceMethod("/user")]
      [RequireRole("USER")]
      public string CheckUser() => "You are an user";

  }
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Api.Content.Authentication;
  using GenHTTP.Api.Protocol;

  using GenHTTP.Engine.Internal;

  using GenHTTP.Modules.Authentication;
  using GenHTTP.Modules.Authentication.ApiKey;
  using GenHTTP.Modules.Functional;

  var auth = ApiKeyAuthentication.Create()
                                 .WithQueryParameter("apiKey")
                                 .Authenticator(AuthenticateRequestAsync);

  var app = Inline.Create()
                  .Get("/admin", [RequireRole("ADMIN")] () => "You are an administrator")
                  .Get("/user", [RequireRole("USER")] () => "You are an user")
                  .Authentication(auth);

  await Host.Create()
            .Handler(app)
            .RunAsync();

  static ValueTask<IUser?> AuthenticateRequestAsync(IRequest request, string apiKey)
  {
      if (apiKey == "abc")
      {
          return new(new ApiKeyUser(apiKey, "ADMIN", "USER"));
      }

      if (apiKey == "bcd")
      {
          return new(new ApiKeyUser(apiKey, "USER"));
      }

      return default;
  }
  ```
{{< /tab >}}

{{< tab >}}
  ```csharp
  using GenHTTP.Api.Content.Authentication;
  using GenHTTP.Api.Protocol;

  using GenHTTP.Engine.Internal;

  using GenHTTP.Modules.Authentication;
  using GenHTTP.Modules.Authentication.ApiKey;
  using GenHTTP.Modules.Controllers;
  using GenHTTP.Modules.Layouting;

  var auth = ApiKeyAuthentication.Create()
                                 .WithQueryParameter("apiKey")
                                 .Authenticator(AuthenticateRequestAsync);

  var app = Layout.Create()
                  .AddController<MyController>("service")
                  .Authentication(auth);

  await Host.Create()
            .Handler(app)
            .RunAsync();

  static ValueTask<IUser?> AuthenticateRequestAsync(IRequest request, string apiKey)
  {
      if (apiKey == "abc")
      {
          return new(new ApiKeyUser(apiKey, "ADMIN", "USER"));
      }

      if (apiKey == "bcd")
      {
          return new(new ApiKeyUser(apiKey, "USER"));
      }

      return default;
  }

  public class MyController
  {

      [ControllerAction(RequestMethod.Get)]
      [RequireRole("ADMIN")]
      public string CheckAdmin() => "You are an administrator";

      [ControllerAction(RequestMethod.Get)]
      [RequireRole("USER")]
      public string CheckUser() => "You are an user";

  }
  ```
{{< /tab >}}

{{< /tabs >}}

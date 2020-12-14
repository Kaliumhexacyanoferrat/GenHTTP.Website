## Authentication

The [authentication module](https://www.nuget.org/packages/GenHTTP.Modules.Authentication/)
allows to restrict access to sections provided 
by handlers to authenticated users only. Independent from the kind of authentication
used, the identified user can be read from the request in the following manner:

```csharp
using GenHTTP.Modules.Authentication;
                        
var displayName = request.GetUser<IUser>()?.DisplayName;
```

The user object for the current request will be determined by the installed authentication
concern. The concern will allow you to add your custom user object there.

## Basic Authentication

Basic authentication can be used by either specifying a list of users and their passwords or
by implementing your custom logic to authenticate and load users (e.g. from a database).

```csharp
var securedContent = Layout.Create();

// specify all users and their password ...
var auth = BasicAuthentication.Create()
                              .Add("Bob", "pw123")
                              .Add("Alice", "123pw");

securedContent.Authentication(auth);

// ... or implement your custom logic
securedContent.Authentication((user, password) => {
   // validate the given credentials here and return your custom user object which needs to implement IUser
   return new BasicAuthenticationUser(user);
});
```

## API Key Authentication

This kind of authentication requires your clients to authenticate themselves using an key sent
with an additional HTTP header (named `X-API-Key`). 

```csharp
var securedContent = Layout.Create();

var auth = ApiKeyAuthentication.Create()
                               .Keys("ABC-123", "BCD-234");

securedContent.Authentication(auth);
```

You may customize both where the key is read from and what keys are valid for authentication:

```csharp
var auth = ApiKeyAuthentication.Create()
                               .WithQueryParameter("key") // read the key from the query ..
                               .WithHeader("key") // .. or a custom header ...
                               .Extractor((r) => ...) // ... or anywhere else
                               .Authenticator(DoAuthenticate);

private IUser? DoAuthenticate(IRequest request, string key)
{
    // do something to check the key (e.g. query a database)
    return new ApiKeyUser(key); // or null, if no access is granted
}
```
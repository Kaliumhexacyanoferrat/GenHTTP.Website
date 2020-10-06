## Authentication

The [authentication module](https://www.nuget.org/packages/GenHTTP.Modules.Authentication/)
allows to restrict access to sections provided 
by handlers to authenticated users only. Independent from the kind of authentication
used, the identified user can be read from the request in the following manner:

```csharp
using GenHTTP.Modules.Authentication;
                        
var displayName = request.GetUser&lt;IUser&gt;()?.DisplayName;
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
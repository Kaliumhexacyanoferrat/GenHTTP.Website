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
                               .Authenticator(DoAuthenticateAsync);

private ValueTask<IUser?> DoAuthenticateAsync(IRequest request, string key)
{
    // do something to check the key (e.g. query a database)
    return new(new ApiKeyUser(key)); // or null, if no access is granted
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

securedContent.Authentication(auth);

// ... or implement your custom logic
securedContent.Authentication((user, password) => {
   // validate the given credentials here and return your custom user object which needs to implement IUser
   return new(new BasicAuthenticationUser(user));
});
```

## Web Authentication

> <span class="note">NOTE</span> The feature described in this section [is deprecated](https://github.com/Kaliumhexacyanoferrat/GenHTTP/issues/496) and will be removed with GenHTTP 9.

This concern will secure your website by adding a login form that needs to be completed by 
the app users to authenticate themselves.Additionally, the concern provides a setup functionality 
that will allow users to create an admin account on the first run of your application. 
The integration is handled by the implementation of an interface passed to the builder.

You can either select a simple way to integrate which will use built-in controllers to
render login forms or pass a more complex integration model that will invoke your custom
controllers which allows to render a more complex login UI.

As the implementation uses advanced features such as controllers, there is an additional
[nuget package](https://www.nuget.org/packages/GenHTTP.Modules.Authentication.Web/) for this.

The following code shows a simple example that will store user and session records in memory:

```csharp
var auth = WebAuthentication.Simple<Integration, User>();

var project = Content.From(Resource.FromString("Hello World"))
                     .Add(auth);

Host.Create()
    .Handler(project)
    .Defaults()
    .Development()
    .Console()
    .Run();

record class User(string Name, string Password) : IUser
{
    public string DisplayName => Name;
}

class Integration : ISimpleWebAuthIntegration<User>
{
    private readonly List<User> _Users = new();

    private readonly Dictionary<string, User> _Sessions = new();

    public ValueTask<bool> CheckSetupRequired(IRequest request) => new(_Users.Count == 0);

    public ValueTask PerformSetup(IRequest request, string username, string password)
    {
        _Users.Add(new(username, password));
        return ValueTask.CompletedTask;
    }

    public ValueTask<User?> PerformLoginAsync(IRequest request, string username, string password)
    {
        return new(_Users.FirstOrDefault(u => u.Name == username && u.Password == password));
    }

    public ValueTask<string> StartSessionAsync(IRequest request, User user)
    {
        var id = Guid.NewGuid().ToString();

        _Sessions[id] = user;

        return new(id);
    }

    public ValueTask<User?> VerifyTokenAsync(IRequest request, string sessionToken) 
    {
        if (_Sessions.TryGetValue(sessionToken, out var user))
        {
            return new(user);
        }

        return new();
    } 

}
```

For the advanced integration you can have a look at the controllers that are used
to render the simple login forms.
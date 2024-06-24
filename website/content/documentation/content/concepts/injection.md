---
title: Parameter Injection
cascade:
  type: docs
---

Frameworks that allow to define custom methods in code (such as [webservices](./webservices), [controllers](./controllers),
or [functional handlers](./functional)) may be extended with custom parameter resolvers which
allow to inject custom types into method invocations.

By default, the framework will inject `IRequest`, `IHandler` and the request body as a `Stream`. This can
be extended by configuring and passing a custom `InjectionRegistry`. The registry
accepts `IParameterIParameterInjector` instances that define what types are supported
and how they are determined from the current request and environment.

The following example shows how to inject the currently authenticated user into a custom method: 

```csharp
var auth = BasicAuthentication.Create()
                              .Add("my_user", "v3rys4v3p4ssw0rd");

var injectors = Injection.Default()
                         .Add(new UserInjector<BasicAuthenticationUser>());

var content = Inline.Create()
                    .Get((BasicAuthenticationUser user) => user.DisplayName)
                    .Injectors(injectors)
                    .Authentication(auth);
```

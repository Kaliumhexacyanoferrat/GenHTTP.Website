## Defaults

The `Defaults()` method provided by the [Practices](https://www.nuget.org/packages/GenHTTP.Modules.Practices/)
module adds some useful concerns to your web server to enable features such as 
[compression](./compression) or [client side caching](./client-caching-validation).
This way, you will have a well configured server instance without the need
of adding everything by yourself.

```csharp
Host.Create()
    .Handler(...)
    .Defaults()
    .Run();
```

If you would like to opt out of a default feature, you may pass a 
flag as needed:

```csharp
Host.Create()
    .Handler(...)
    .Defaults(compression: false)
    .Run();
```
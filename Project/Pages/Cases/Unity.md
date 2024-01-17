## Introduction

With the GenHTTP framework you can easily define and host REST web services that support your games
written with Unity. As webservices are also written in C# / .NET and can be set up using project 
templates in a couple of minutes they take less of your time compared to learning advanced technologies
such as ASP.NET.

## Creating a Project

A new web service API project can be created by utilizing the project templates of the framework. To create
a new project, run the following commands in your terminal:

```
dotnet new -i GenHTTP.Templates

mkdir MyService
cd MyService
dotnet new genhttp-webservice
```

This will create a new, already working project that you can modify and extend. It can either be
edited in [Visual Studio](https://visualstudio.microsoft.com/) or with an editor of your choice.
To run the project, execute:

```
dotnet run
```

This will make your server available on http://localhost:8080/.

## Next Steps

The [documentation](/documentation/content/) will show you how to use advanced features
such as API key authentication or how to [host your app](/documentation/hosting/).
---
title: Hosting
weight: 5
description: 'Host web applications written in C# using the .NET docker images.'
cascade:
  type: docs
---
## Hosting with Docker

[Docker](https://www.docker.com/) allows to build, package and run your applications
developed with GenHTTP on any server, whether it is on your NAS at home or a Kubernetes
cluster in the cloud.

As GenHTTP is built on top of .NET, we can use the base images provided by Microsoft
to setup our build chain. For a list of available base images,
see their [Docker Hub](https://hub.docker.com/r/microsoft/dotnet-sdk) page.

## Creating a new Dockerfile

Create a new file named `Dockerfile` in the root directory of your repository and paste the following content:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:11.0-alpine AS build

# uncomment those lines to enable globalization / localization features
# RUN apk add --no-cache icu-libs tzdata
# ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /source

# copy csproj and restore as distinct layers
COPY Project/*.csproj .
RUN dotnet restore -r linux-musl-x64

# copy and publish app and libraries
COPY Project/ .
RUN dotnet publish -c release -o /app -r linux-musl-x64 --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:11.0-alpine

# or FROM mcr.microsoft.com/dotnet/aspnet:11.0-alpine for Kestrel

ENV DOTNET_EnableDiagnostics=0 \
    DOTNET_gcServer=1 \
    DOTNET_TieredPGO=1 \
    DOTNET_ReadyToRun=1

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Project.dll"]

EXPOSE 8080
```

This assumes that you named your project `Project`. With this file you can use
the commands in the previous section to build and run your project. GenHTTP currently
targets .NET 10 and .NET 11, so `10.0` images work just as well - pick whichever
matches the target framework of your project.

{{< callout type="info" >}}
  If you are hosting on the [Ioxide engine](../server/engines/ioxide/), keep in mind it
  needs .NET 11 and relies on `io_uring`, which is provided by the host's Linux kernel, not
  the container image - it is unavailable on hosts with an older kernel or on container
  runtimes that block the `io_uring` syscalls (e.g. some hardened or gVisor-based
  environments), regardless of which .NET base image you pick.
{{< /callout >}}

## Managing dependencies

Typically, your web application will have some dependencies such as databases
or a redis server. [docker compose](https://docs.docker.com/compose/gettingstarted/)
allows you to define and maintain the whole infrastructure needed by your app
in a single file.

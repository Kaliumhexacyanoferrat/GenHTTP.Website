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

## Building Template Apps

If you created your application using a [project template](../content/templates/),
you will see that matching Docker files have already been created. Building and
running your app is as easy as:

```bash
# creates an image named "myproject"
docker build -f Dockerfile -t myproject .

# runs your application
docker run -p 8080:8080 -d myproject
```

You should be able to access your app via http://localhost:8080. If you run those
commands on the machine you would like to host your application on (e.g. a Linux server)
you will not need to install the .NET SDK or any other dependencies besides Docker itself.

## Creating a new Dockerfile

If you did not use a project template, create a new file named `Dockerfile` in the
root directory of your repository and paste the following content:

```dockerfile
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG TARGETARCH
WORKDIR /source

COPY --link Project/*.csproj .
RUN dotnet restore -a "$TARGETARCH"

COPY --link Project/. .
RUN dotnet publish --no-restore -a "$TARGETARCH" -o /app

# Enable globalization and time zones:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/enable-globalization.md

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
EXPOSE 8080
WORKDIR /app
COPY --link --from=build /app .
USER $APP_UID
ENTRYPOINT ["./Project"]
```

This assumes that you named your project `Project`. With this file you can use
the commands in the previous section to build and run your project.

## Managing dependencies

Typically, your web application will have some dependencies such as databases
or a redis server. [docker compose](https://docs.docker.com/compose/gettingstarted/)
allows you to define and maintain the whole infrastructure needed by your app
in a single file.

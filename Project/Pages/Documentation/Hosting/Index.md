## Hosting with Docker

[Docker](https://www.docker.com/) is the preferred way to host
web applications created with the GenHTTP application framework. .NET allows
to create images for various platforms and CPU architectures. For a list of available base images,
see their [Docker Hub](https://hub.docker.com/_/microsoft-dotnet-core-sdk/) page.

To create an image for your web application, create a new file named `Dockerfile` in the
root directory of your repository. The following example is for an x64 image
running on Linux:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY Project/*.csproj .
RUN dotnet restore -r linux-musl-x64

# copy and publish app and libraries
COPY Project/ .
RUN dotnet publish -c release -o /app -r linux-musl-x64 --no-restore /p:PublishTrimmed=true /p:TrimMode=Link

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine-amd64
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["./Project"]

EXPOSE 8080
```

To build this docker image, run:

```bash
sudo docker build -t project -f Dockerfile .
```

This will create a new image named `project`. To run a new container
with this image, execute the following command:

```bash
sudo docker run -d -p 8080:8080 project
```

You should now be able to access the application via http://localhost:8080.
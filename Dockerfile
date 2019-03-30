FROM mcr.microsoft.com/dotnet/core/sdk:3.0-stretch-arm32v7 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY Project/*.csproj ./Project/
WORKDIR /app/Project
RUN dotnet restore

# copy and build app and libraries
WORKDIR /app/
COPY Project/. ./Project/
WORKDIR /app/Project
# add IL Linker package
RUN dotnet add package ILLink.Tasks -v 0.1.5-preview-1841731 -s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json
RUN dotnet publish -c Release -r linux-arm -o out /p:ShowLinkerSizeComparison=true

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.0-stretch-slim-arm32v7 AS runtime
WORKDIR /app
COPY --from=build /app/Project/out ./
ENTRYPOINT ["./Project"]

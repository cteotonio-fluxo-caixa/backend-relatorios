#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_HTTP_PORTS=8082
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_VERSION=8.0.1
ENV ASPNET_VERSION=8.0.1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FluxoCaixa.Relatorio.API/FluxoCaixa.Relatorio.API.csproj", "FluxoCaixa.Relatorio.API/"]
RUN dotnet restore "./FluxoCaixa.Relatorio.API/./FluxoCaixa.Relatorio.API.csproj"
COPY . .
WORKDIR "/src/FluxoCaixa.Relatorio.API"
RUN dotnet build "./FluxoCaixa.Relatorio.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FluxoCaixa.Relatorio.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluxoCaixa.Relatorio.API.dll"]
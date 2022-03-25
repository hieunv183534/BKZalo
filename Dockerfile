#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Dockerfile

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["BKZalo.Api/BKZalo.Api.csproj", "BKZalo.Api/"]
COPY ["BKZalo.Core/BKZalo.Core.csproj", "BKZalo.Core/"]
COPY ["BKZalo.Infrastructure/BKZalo.Infrastructure.csproj", "BKZalo.Infrastructure/"]
RUN dotnet restore "BKZalo.Api/BKZalo.Api.csproj"
COPY . .
WORKDIR "/src/BKZalo.Api"
RUN dotnet build "BKZalo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BKZalo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BKZalo.Api.dll"]
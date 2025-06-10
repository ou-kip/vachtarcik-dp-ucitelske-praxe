# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["AuthorizationService/AuthorizationService.csproj", "AuthorizationService/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Core.Database/Core.Database.csproj", "Core.Database/"]
COPY ["Core.Domain/Core.Domain.csproj", "Core.Domain/"]
COPY ["Core.Infrastructure/Core.Infrastructure.csproj", "Core.Infrastructure/"]
COPY ["Core.Messaging/Core.Messaging.csproj", "Core.Messaging/"]
COPY ["Identity/Identity.csproj", "Identity/"]
RUN dotnet restore "./AuthorizationService/AuthorizationService.csproj"
COPY . .
WORKDIR "/src/AuthorizationService"
RUN dotnet build "./AuthorizationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthorizationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final

#COPY ./Ssl/praxeosucz.pfx /app/Ssl/praxeosucz.pfx
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizationService.dll"]
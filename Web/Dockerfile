FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
RUN apk --no-cache add curl
USER app
WORKDIR /app
ENV ASPNETCORE_URLS="http://+:80;http://+:5000"
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Web.csproj", "Web/"]
RUN dotnet restore "./Web/Web.csproj"
COPY [".", "./Web/"]
WORKDIR "/src/Web"
RUN dotnet build "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
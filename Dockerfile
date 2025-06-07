# Imagen base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Imagen de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["LolHubMexico.API/LolHubMexico.API.csproj", "LolHubMexico.API/"]
COPY ["LolHubMexico.Application/LolHubMexico.Application.csproj", "LolHubMexico.Application/"]
COPY ["LolHubMexico.Domain/LolHubMexico.Domain.csproj", "LolHubMexico.Domain/"]
COPY ["LolHubMexico.Infrastructure/LolHubMexico.Infrastructure.csproj", "LolHubMexico.Infrastructure/"]
COPY . .
WORKDIR "/src/LolHubMexico.API"
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

# Publicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LolHubMexico.API.dll"]
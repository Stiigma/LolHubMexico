# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar los .csproj
COPY LolHubMexico.Application/LolHubMexico.Application.csproj ./LolHubMexico.Application/
COPY LolHubMexico.Domain/LolHubMexico.Domain.csproj ./LolHubMexico.Domain/
COPY LolHubMexico.Infrastructure/LolHubMexico.Infrastructure.csproj ./LolHubMexico.Infrastructure/
COPY lolhubmexico/LolHubMexico.API.csproj ./lolhubmexico/

# Restaurar
RUN dotnet restore ./lolhubmexico/LolHubMexico.API.csproj

# Copiar todo el proyecto
COPY . .

# Publicar
WORKDIR /src/lolhubmexico
RUN dotnet publish "LolHubMexico.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "LolHubMexico.API.dll"]

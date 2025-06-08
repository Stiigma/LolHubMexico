# Fase 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar proyectos
COPY lolhubmexico.sln .
COPY lolhubmexico/ ./lolhubmexico/
COPY LolHubMexico.Application/ ./LolHubMexico.Application/
COPY LolHubMexico.Infrastructure/ ./LolHubMexico.Infrastructure/
COPY LolHubMexico.Domain/ ./LolHubMexico.Domain/

# Restaurar dependencias
WORKDIR /src/lolhubmexico
RUN dotnet restore LolHubMexico.csproj

# Publicar con dependencias necesarias
RUN dotnet publish LolHubMexico.csproj -c Release -o /app/publish /p:UseAppHost=false

# Fase 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiar archivos publicados
COPY --from=build /app/publish .

# Abrir puerto
EXPOSE 8080

# Iniciar la app
ENTRYPOINT ["dotnet", "LolHubMexico.dll"]

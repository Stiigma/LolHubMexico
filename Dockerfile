# Fase de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar carpetas del proyecto
COPY lolhubmexico/ ./lolhubmexico/
COPY LolHubMexico.Application/ ./LolHubMexico.Application/
COPY LolHubMexico.Infrastructure/ ./LolHubMexico.Infrastructure/
COPY LolHubMexico.Domain/ ./LolHubMexico.Domain/

# Restaurar dependencias
WORKDIR /src/lolhubmexico
RUN dotnet restore "LolHubMexico.csproj"

# Publicar
RUN dotnet publish "LolHubMexico.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Fase final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "LolHubMexico.dll"]

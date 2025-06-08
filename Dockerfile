# Fase de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar los proyectos por separado
COPY ["LolHubMexico.API/LolHubMexico.API.csproj", "LolHubMexico.API/"]
COPY ["LolHubMexico.Application/LolHubMexico.Application.csproj", "LolHubMexico.Application/"]
COPY ["LolHubMexico.Infrastructure/LolHubMexico.Infrastructure.csproj", "LolHubMexico.Infrastructure/"]
COPY ["LolHubMexico.Domain/LolHubMexico.Domain.csproj", "LolHubMexico.Domain/"]

# Restaurar dependencias
RUN dotnet restore "LolHubMexico.API/LolHubMexico.API.csproj"

# Copiar todo el código fuente
COPY . .

# Establecer carpeta de trabajo para la API
WORKDIR "/src/LolHubMexico.API"

# PUBLICAR el proyecto (esto genera todos los archivos necesarios)
RUN dotnet publish "LolHubMexico.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Fase final: imagen de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiar archivos publicados desde la fase de build
COPY --from=build /app/publish .

# Abrir el puerto
EXPOSE 8080

# Punto de entrada
ENTRYPOINT ["dotnet", "LolHubMexico.API.dll"]

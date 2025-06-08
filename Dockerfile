# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["LolHubMexico.API/LolHubMexico.API.csproj", "LolHubMexico.API/"]
COPY ["LolHubMexico.Application/LolHubMexico.Application.csproj", "LolHubMexico.Application/"]
COPY ["LolHubMexico.Infrastructure/LolHubMexico.Infrastructure.csproj", "LolHubMexico.Infrastructure/"]
COPY ["LolHubMexico.Domain/LolHubMexico.Domain.csproj", "LolHubMexico.Domain/"]
RUN dotnet restore "LolHubMexico.API/LolHubMexico.API.csproj"
COPY . .
WORKDIR "/src/LolHubMexico.API"
RUN dotnet publish "LolHubMexico.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LolHubMexico.API.dll"]
EXPOSE 8080

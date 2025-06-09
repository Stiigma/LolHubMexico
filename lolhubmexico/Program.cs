using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using LolHubMexico.Infrastructure.Data;
using LolHubMexico.Infrastructure.Repositories.UserRepository;
using LolHubMexico.Application.UserServices;
using LolHubMexico.Domain.Repositories.UserRepository;
using LolHubMexico.Application.Middlaware;
using LolHubMexico.Infrastructure.Security;
using LolHubMexico.Domain.Repositories.TeamRepository;
using LolHubMexico.Infrastructure.Repositories.TeamRepository;
using LolHubMexico.Application;
//using Microsoft.AspNet.SignalR.WebSockets;
using LolHubMexico.WebSockets;
using LolHubMexico.Notifiers;
using LolHubMexico.Domain.Notifications;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using LolHubMexico.Application.ScrimService;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Infrastructure.Services;
using LolHubMexico.Application.PlayerService;
using LolHubMexico.Domain.Repositories.ScrimRepository;
using LolHubMexico.Domain.Repositories.PlayerRepository;
using LolHubMexico.Infrastructure.Repositories.PlayerRepository;
using LolHubMexico.Infrastructure.Repositories.ScrimRepository;
using LolHubMexico.Application.ScrimDetailsService;

var builder = WebApplication.CreateBuilder(args);

// Obtener puerto desde variable de entorno para Cloud Run
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LolHubMexico.API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Firebase
var firebaseCredentialPath = Path.Combine(Directory.GetCurrentDirectory(), "lolhubmexico-9fa9f-firebase-adminsdk-fbsvc-db5ca683e3.json");
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(firebaseCredentialPath)
});

// DB Context
builder.Services.AddDbContext<ContextDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConectionDB")));

// Repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IScrimRepository, ScrimRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ITeamInvitationRepository, TeamInvitationRepository>();
builder.Services.AddScoped<IDetailsScrimRepository, DetailsScrimRepository>();
builder.Services.AddScoped<ScrimDetailServices>();

// Servicios
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<ScrimService>();
builder.Services.AddScoped<IRiotService, RiotService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<TeamInvitationService>();

// Notificaciones y WebSocket
builder.Services.AddSingleton<WebSocketConnectionManager>();
builder.Services.AddSingleton<LolHubMexico.WebSockets.WebSocketHandler>();
builder.Services.AddScoped<INotifier, TeamInvitationNotifier>();
builder.Services.AddScoped<TeamInvitationNotifier>();
builder.Services.AddSingleton<INotifierFactory, NotifierFactory>();

var app = builder.Build();

// Middleware
app.UseCors("AllowAllOrigins");
app.UseMiddleware<ErrorHandlerMiddleware>();
//app.UseHttpsRedirection();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LolHubMexico.API v1");
    });
}

// WebSockets
app.UseWebSockets();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var handler = context.RequestServices.GetRequiredService<WebSocketHandler>();
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var userId = int.Parse(context.Request.Query["userId"]);
            await handler.HandleConnectionAsync(userId, socket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});

app.MapControllers();

app.Run();
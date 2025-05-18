using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using LolHubMexico.Infrastructure.Data;
using LolHubMexico.Infrastructure.Repositories.UserRepository;
using LolHubMexico.Application.UserService;
using LolHubMexico.Domain.Repositories.UserRepository;
using LolHubMexico.Application.Middlaware;
using LolHubMexico.Infrastructure.Security;
using LolHubMexico.Domain.Repositories.TeamRepository;
using LolHubMexico.Infrastructure.Repositories.TeamRepository;
using LolHubMexico.Application.TeamService;
//using Microsoft.AspNet.SignalR.WebSockets;
using LolHubMexico.API.WebSockets;
using LolHubMexico.API.Notifiers;
using LolHubMexico.Domain.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LolHubMexico.API", Version = "v1" });
});


// Adding DB context
builder.Services.AddDbContext<ContextDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConectionDB")));

// Registering services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<WebSocketConnectionManager>();
builder.Services.AddSingleton<LolHubMexico.API.WebSockets.WebSocketHandler>();
builder.Services.AddScoped<INotifier, TeamInvitationNotifier>();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<ITeamInvitationRepository, TeamInvitationRepository>();
builder.Services.AddScoped<TeamInvitationService>();
builder.Services.AddSingleton<INotifierFactory, NotifierFactory>();
builder.Services.AddScoped<TeamInvitationNotifier>(); // y otros notifiers

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
var app = builder.Build();
app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlerMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Habilitar Swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LolHubMexico.API v1"); // Definir el endpoint de Swagger
    });
}

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

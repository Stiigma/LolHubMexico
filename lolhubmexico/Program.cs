using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using LolHubMexico.Infrastructure.Data;
using LolHubMexico.Infrastructure.Repositories.UserRepository;
using LolHubMexico.Application.UserService;
using LolHubMexico.Domain.Repositories.UserRepository;
using LolHubMexico.Application.Middlaware;

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

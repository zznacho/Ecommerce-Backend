// Program.cs
using MiApp.Application;
using MiApp.Infrastructure;
using MiApp.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrar las capas de la Arquitectura Limpia
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseExceptionHandler(); // <-- DEBE SER EL PRIMERO

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
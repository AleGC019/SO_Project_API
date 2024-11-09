using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SO_API_REST.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura el DbContext con la opción de resiliencia de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 26)),
        mysqlOptions =>
        {
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,       // Número máximo de intentos
                maxRetryDelay: TimeSpan.FromSeconds(10), // Tiempo máximo entre intentos
                errorNumbersToAdd: null // Opcional: Especificar códigos de error transitorios
            );
        }));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
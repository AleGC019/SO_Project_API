using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RM_API.API.Utils;
using RM_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddConsole();

// Configuración de servicios
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorizationPolicies();
builder.Services.RegisterScopedServices();
builder.Services.RegisterSingletonServices(builder.Configuration);

// Configuración del DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLazyLoadingProxies()
);

// Configuración de JSON
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración de CORS
app.UseCors("AllowAllOrigins");

// Test de conexión a la base de datos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var dbTestUtil = new DatabaseTestUtil(context);
    if (!dbTestUtil.TestConnection())
    {
        Console.WriteLine("❌ Error al conectar a la base de datos.");
        return;
    }

    Console.WriteLine("✅ Conexión a la base de datos exitosa.");
}

// Middlewares de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Swagger en desarrollo y producción
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.RoutePrefix = string.Empty;
    });
}

// Mapear controladores
app.MapControllers();

// Run
app.Run();
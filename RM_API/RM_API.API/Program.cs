using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RM_API.API.Utils;
using RM_API.Core.Entities;
using RM_API.Data;
using RM_API.Data.Repositories;
using RM_API.Data.Repositories.Interfaces;
using RM_API.Service.Services;
using RM_API.Service.Services.Interfaces;
using RM_API.Service.Tools;
using DateTimeConverter = RM_API.API.Utils.DateTimeConverter;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy that allows all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.Configure<JwtSettings>(jwtSettings);
var secretKey = jwtSettings["SecretKey"];
if (secretKey != null)
{
    var key = Encoding.UTF8.GetBytes(secretKey);

    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
}
else
{
    throw new Exception("JwtSettings:SecretKey is required in appsettings.json");
}

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole(RoleName.ADMIN.ToString()));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});


// Register JwtTokenGenerator in dependency injection
builder.Services.AddSingleton<JwtTokenGenerator>();

// Register TimeZoneTool in dependency injection
builder.Services.AddSingleton<TimeZoneTool>();

// Register DatabaseTestUtil for dependency injection
builder.Services.AddSingleton<DatabaseTestUtil>();

// Register Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter("yyyy-MM-ddTHH:mm:ssZ")); // ISO 8601 format
    });

// Register services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IHouseRepository, HouseRepository>();
builder.Services.AddScoped<IHouseService, HouseService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAllOrigins"); // Enable the AllowAllOrigins policy

// Test the database connection
var dbTestService = app.Services.GetRequiredService<DatabaseTestUtil>();
var isConnected = dbTestService.TestConnection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment() && isConnected)
{
    Console.WriteLine("✅ Conexión a la base de datos establecida correctamente.");
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    Console.WriteLine("❌ Error al conectar a la base de datos.");
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
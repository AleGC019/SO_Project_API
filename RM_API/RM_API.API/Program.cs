using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RM_API.Data;
using System.Text;
using RM_API.API.Utils;
using RM_API.Core.Interfaces;
using RM_API.Core.Interfaces.IRole;
using RM_API.Core.Interfaces.IUser;
using RM_API.Data.Repositories;
using RM_API.Service.Services;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var secretKey = jwtSettings["SecretKey"];
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

builder.Services.AddAuthorization();

// Register JwtTokenGenerator in dependency injection
builder.Services.AddSingleton<JwtTokenGenerator>();

// Register DatabaseTestUtil for dependency injection
builder.Services.AddSingleton<DatabaseTestUtil>();

// Register Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add other services
builder.Services.AddControllers();

// Register services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Test the database connection
var dbTestService = app.Services.GetRequiredService<DatabaseTestUtil>();
bool isConnected = dbTestService.TestConnection();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
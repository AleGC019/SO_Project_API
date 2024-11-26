using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RM_API.API.Utils.JWT;
using RM_API.Core.Entities;
using RM_API.Data.Repositories;
using RM_API.Data.Repositories.Interfaces;
using RM_API.Service;
using RM_API.Service.Services;
using RM_API.Service.Services.Interfaces;
using RM_API.Service.Tools;

namespace RM_API.API.Utils;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];

        if (string.IsNullOrEmpty(secretKey))
            throw new Exception("JwtSettings:SecretKey is required in appsettings.json");

        var key = Encoding.UTF8.GetBytes(secretKey);

        services.AddAuthentication(options =>
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
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar políticas de CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    public static IServiceCollection RegisterScopedServices(this IServiceCollection services)
    {
        // Registro de servicios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();
        
        services.AddScoped<IHouseRepository, HouseRepository>();
        services.AddScoped<IHouseService, HouseService>();
        
        services.AddScoped<IPermitRepository, PermitRepository>();
        services.AddScoped<IPermitService, PermitService>();
        
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<IEntryService, EntryService>();

        return services;
    }

    public static IServiceCollection RegisterSingletonServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Registro de herramientas
        services.AddSingleton<JwtTokenGenerator>();
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var appTimeZone = configuration["AppSettings:TimeZone"]; // "America/Mexico_City"
            return new TimeZoneTool(appTimeZone);
        });

        return services;
    }
    
    public static IServiceCollection RegisterTransientServices(this IServiceCollection services)
    {
        // Registro de servicios transitorios
        //services.AddTransient<DatabaseTestUtil>();

        return services;
    }

    public static IServiceCollection ConfigureAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole(RoleName.ADMIN.ToString()));
            options.AddPolicy("Residents",
                policy => policy.RequireRole(RoleName.ADMIN.ToString(), RoleName.RES.ToString()));
            options.AddPolicy("Security",
                policy => policy.RequireRole(RoleName.ADMIN.ToString(), RoleName.SEC.ToString()));
            options.AddPolicy("Vigilante", policy => policy.RequireRole(RoleName.SEC.ToString()));
        });

        return services;
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Dominio.Interfaces;
using Infraestrutura.Dados.Contexts;
using Infraestrutura.Dados.Services;
using Infraestrutura.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestrutura;

public static class ServiceConfigurator
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        IdentityModelEventSource.ShowPII = true;

        services.AddScoped<ITokenServico, TokenServico>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<GrupoTarefasServico>();
        services.AddScoped<TarefaServico>();

        services.ConfigureDatabaseContexts(configuration);

        services.ConfigureIdentity();
        services.ConfigureAuthentication(configuration);

        services.AddControllers();
    }

    private static void ConfigureDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var tarefasConnection = configuration.GetConnectionString("TarefasConnection");

        var identityConnection = configuration.GetConnectionString("IdentityConnection");

        services.AddDbContext<TarefasContext>(options => options.UseSqlite(tarefasConnection));
        services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlite(identityConnection));
    }

    private static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenKey = configuration["Token:Key"];
        if (string.IsNullOrEmpty(tokenKey))
        {
            throw new InvalidOperationException("Token:Key configuration is missing.");
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey))
        {
            KeyId = "my-key-id"
        };

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
                ValidIssuer = configuration["Token:Issuer"],
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = signingKey,
                ClockSkew = TimeSpan.Zero 
            };
        });
    }

}

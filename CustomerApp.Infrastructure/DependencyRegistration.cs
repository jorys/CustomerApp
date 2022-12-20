using CustomerApp.Application.Interfaces;
using CustomerApp.Infrastructure.EmailSenders;
using CustomerApp.Infrastructure.JwtTokenGenerators;
using CustomerApp.Infrastructure.PasswordHashers;
using CustomerApp.Infrastructure.Repositories;
using CustomerApp.Infrastructure.TokenGenerators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CustomerApp.Application;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationDependencies(configuration);
        services.Configure<PasswordHashSettings>(configuration.GetSection(nameof(PasswordHashSettings)));
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IRepository, InMemoryRepository>();
        services.Configure<EmailSenderSettings>(configuration.GetSection(nameof(EmailSenderSettings)));
        services.AddScoped<IEmailSender, EmailSender>();
        return services;
    }

    static IServiceCollection AddAuthenticationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = 
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                });
        return services;
    }
}

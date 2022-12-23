using CustomerApp.RestApi.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CustomerApp.RestApi;

public static class DependencyRegistration
{
    public static IServiceCollection AddRestApiDependencies(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Customer App",
                Description = "An example of DDD-CQRS-Clean architecture implementation."
            });
            options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
            var projectName = Assembly.GetExecutingAssembly().GetName().Name;
            var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, $"{projectName}.xml");
            options.IncludeXmlComments(xmlCommentsFilePath);
            options.CustomSchemaIds(x => x?.FullName?.Split('.').Last().Replace("+", "."));

            // JWT token
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id =  JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddControllers();

        return services;
    }
}
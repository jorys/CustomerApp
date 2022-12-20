using CustomerApp.Application.Handlers.Authentication;
using CustomerApp.Application.Handlers.Customers;
using CustomerApp.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerApp.Application;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<GetCustomerHandler>();
        services.AddScoped<DeleteCustomerHandler>();
        services.AddScoped<UpdateCustomerHandler>();
        services.Configure<ResetPasswordSettings>(configuration.GetSection(nameof(ResetPasswordSettings)));
        services.AddScoped<ForgotPasswordHandler>();
        services.AddScoped<ResetPasswordHandler>();
        return services;
    }
}

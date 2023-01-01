using CustomerApp.Application.Handlers.Authentication;
using CustomerApp.Application.Handlers.Authentication.Settings;
using CustomerApp.Application.Handlers.Customers;
using CustomerApp.Application.Handlers.CustomerStocks;
using CustomerApp.Application.Handlers.CustomerStocks.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerApp.Application;

public static class DependencyRegistration
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<ForgotPasswordHandler>();
        services.Configure<ResetPasswordSettings>(configuration.GetSection(nameof(ResetPasswordSettings)));
        services.AddScoped<ResetPasswordHandler>();

        services.AddScoped<GetCustomerHandler>();
        services.AddScoped<DeleteCustomerHandler>();
        services.AddScoped<UpdateCustomerHandler>();
        services.AddScoped<UpdatePasswordHandler>();

        services.AddScoped<GetStocksHandler>();
        services.Configure<ConcurrencyRetrySettings>(configuration.GetSection(nameof(ConcurrencyRetrySettings)));
        services.AddScoped<AddToStockHandler>();
        services.AddScoped<RemoveFromStockHandler>();

        return services;
    }
}

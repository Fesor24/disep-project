using GadgetHub.DataAccess.Abstractions;
using GadgetHub.DataAccess.Implementation;
using GadgetHub.Services.Abstractions;
using GadgetHub.Services.Implementation;
using PayStack.Net;

namespace GadgetHub.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IShoppingCartService, ShoppingCartService>();

        services.AddScoped<IOrderService, OrderService>();

        services.AddScoped<IPaymentService, PaymentService>();

        services.AddTransient(sp => new PayStackApi(config["PayStack:Secret"]));

        return services;
    }
}

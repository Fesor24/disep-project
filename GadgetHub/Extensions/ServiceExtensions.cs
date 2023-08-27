using GadgetHub.DataAccess.Abstractions;
using GadgetHub.DataAccess.Implementation;
using GadgetHub.Services.Abstractions;
using GadgetHub.Services.Implementation;

namespace GadgetHub.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IShoppingCartService, ShoppingCartService>();

        return services;
    }
}

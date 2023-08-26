using GadgetHub.Data;
using GadgetHub.Data.DataSeed;
using Microsoft.EntityFrameworkCore;

namespace GadgetHub.Extensions;

public static class ApplicationExtensions
{
    public static async Task DatabaseInit(this WebApplication app)
    {
        using(var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                await context.Database.MigrateAsync();

                await DataSeed.PopulateAsync(context, loggerFactory);
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogError("An error occurred. Details:{error}", ex.StackTrace);
            }
        }
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}

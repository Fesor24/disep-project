using GadgetHub.Data;
using GadgetHub.Data.DataSeed;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

                string inMemoryDb = app.Configuration["InMemoryDb"];

                if(inMemoryDb.IsNullOrEmpty() || inMemoryDb != "true")
                {
                    await context.Database.MigrateAsync();
                }

                await DataSeed.PopulateAsync(context, loggerFactory);
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogError($"An error occurred. Details:{ex.Message} {ex.StackTrace}");
            }
        }
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration config)
    {
        string useInMemoryDb = config["InMemoryDb"];

        if (useInMemoryDb.IsNullOrEmpty())
        {
            return services;
        }

        if(useInMemoryDb == "true")
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("GadgetHub");
            });
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
        }
        return services;
    }
}

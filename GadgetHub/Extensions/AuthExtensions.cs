using GadgetHub.Data;
using GadgetHub.Entities.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace GadgetHub.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthenticaton(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication("AuthCookie")
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/Account/Login";
                options.LoginPath = "/Account/Login";
                options.Cookie.Name = "AuthCookie";
            });
            

        return services;
    }
}

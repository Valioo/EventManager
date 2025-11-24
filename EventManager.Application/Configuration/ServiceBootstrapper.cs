using EventManager.Application.Contracts;
using EventManager.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace EventManager.Application.Configuration;

public static class ServiceBootstrapper
{
    public static IServiceCollection BootstrapApplication(this IServiceCollection services)
    {
        return services
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IUserService, UserService>();
    }

    public static void AddJwt(this IServiceCollection services, IConfigurationSection jwtSection)
    {
        var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RoleClaimType = ClaimTypes.Role
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Administrator"));

            options.AddPolicy("OrganizerOnly", policy =>
                policy.RequireRole("Organizer"));

            options.AddPolicy("AdminOrOrganizer", policy =>
                policy.RequireRole("Administrator", "Organizer"));
        });
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Infrastructure.Authentication;
using SuBilgiSurveyBackend.Infrastructure.Identity;
using SuBilgiSurveyBackend.Infrastructure.Jwt;
using SuBilgiSurveyBackend.Infrastructure.Persistence;
using SuBilgiSurveyBackend.Infrastructure.Settings;

namespace SuBilgiSurveyBackend.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddJwtAuthentication(configuration);

        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            )
        );

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IIdentityService, IdentityManager>();
        services.AddScoped<ITokenService, TokenManager>();

        return services;
    }
}
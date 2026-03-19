using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Infrastructure.Settings;

namespace SuBilgiSurveyBackend.Infrastructure.Authentication;

public static class JwtAuthenticationExtensions
{
    /// <summary>
    /// JwtSettings bölümünden access/refresh secret, issuer ve audience okuyarak JWT Bearer şemalarını kaydeder.
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = new JwtSettings();
        configuration.GetSection("Jwt").Bind(jwt);

        var accessSecret = jwt.AccessTokenSecret ?? string.Empty;
        var refreshSecret = jwt.RefreshTokenSecret ?? string.Empty;
        var issuer = jwt.Issuer ?? string.Empty;
        var audience = jwt.Audience ?? string.Empty;

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessSecret)),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddJwtBearer("RefreshToken", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshSecret)),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("RefreshToken", policy => policy.RequireClaim(ClaimExtensions.NameIdentifier));

        return services;
    }
}

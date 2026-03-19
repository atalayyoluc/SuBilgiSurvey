using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SuBilgiSurveyBackend.Application.Common.Behaviors;
using SuBilgiSurveyBackend.Application.Mappings;
using System.Reflection;

namespace SuBilgiSurveyBackend.Application;

public static class ConfigureService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(AuthenticatedUserBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddMapperConfiguration();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
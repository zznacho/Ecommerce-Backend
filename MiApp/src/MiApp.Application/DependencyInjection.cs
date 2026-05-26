// DependencyInjection.cs
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiApp.Application.Common.Behaviors;

namespace MiApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Registrar MediatR asignando el ensamblado actual
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(assembly);
            // Registrar nuestro interceptor de validaciones automáticas
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Registrar todos los validadores de FluentValidation del proyecto
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
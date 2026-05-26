using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using MiApp.Application.Common.Behaviors;
using System.Reflection;

namespace MiApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // 1. Registrar todos los validators de FluentValidation que creamos en este proyecto
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // 2. Registrar MediatR inyectando de forma automática el ValidationBehavior
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            
            // El behavior se va a ejecutar ANTES de cada handler automáticamente
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Registrar casos de uso específicos
        services.AddScoped<MiApp.Application.UseCases.LoginUseCase>();
        services.AddScoped<MiApp.Application.UseCases.RegisterUseCase>();

        return services;
    }
}
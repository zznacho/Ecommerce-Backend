// DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiApp.Application.Interfaces; 
using MiApp.Domain.Interfaces;
using MiApp.Infrastructure.Persistence;

// Usings clave para que encuentre tus repositorios viejos y nuevos:
using MiApp.Infrastructure.Repositories; 
using MiApp.Infrastructure.Persistence.Repositories;

namespace MiApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Registrar el DbContext utilizando SQLite (Scoped lifetime automático)
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // 2. Registrar las implementaciones concretas del negocio
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>(); 
        services.AddScoped<IUserRepository, UserRepository>();   
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Servicio de tokens JWT
        services.AddScoped<MiApp.Application.Interfaces.ITokenService, MiApp.Infrastructure.Services.JwtTokenService>();

        return services;
    }
}
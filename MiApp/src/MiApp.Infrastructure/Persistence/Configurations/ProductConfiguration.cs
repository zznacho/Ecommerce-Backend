// Persistence/Configurations/ProductConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiApp.Domain.Entities;

namespace MiApp.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        // Declaramos que el GUID se genera en el Dominio (Factory Method), no en la BD
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
               .ValueGeneratedNever();

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Description)
               .HasMaxLength(2000);

        // SQLite no requiere especificar decimal(18,2) estrictamente, pero lo dejamos mapeado de forma segura
        builder.Property(p => p.Price)
               .HasConversion<double>() // Mapeo seguro para la compatibilidad numérica de SQLite
               .IsRequired();

        builder.Property(p => p.Stock)
               .IsRequired()
               .HasDefaultValue(0);

        builder.Property(p => p.IsActive)
               .IsRequired()
               .HasDefaultValue(true);

        // Índice para búsquedas rápidas por nombre, tal como pide la sección 4.1
        builder.HasIndex(p => p.Name);
    }
}
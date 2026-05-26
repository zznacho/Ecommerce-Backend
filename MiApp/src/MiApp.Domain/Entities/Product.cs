// Entities/Product.cs
using MiApp.Domain.Exceptions;

namespace MiApp.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }

    // Constructor privado requerido por Entity Framework Core para la materialización posterior
    private Product() { }

    // Factory method - Única vía de creación para garantizar que un producto sea inicialmente válido
    public static Product Create(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre del producto es obligatorio.");
        if (price <= 0)
            throw new DomainException("El precio debe ser mayor a cero.");
        if (stock < 0)
            throw new DomainException("El stock no puede ser negativo.");

        return new Product
        {
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            Price = price,
            Stock = stock,
            IsActive = true
        };
    }

    // Comportamiento explícito del Dominio
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainException("El precio debe ser mayor a cero.");
        Price = newPrice;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La cantidad a añadir debe ser mayor a cero.");
        Stock += quantity;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La cantidad a remover debe ser mayor a cero.");
        if (Stock < quantity)
            throw new DomainException($"Stock insuficiente. Disponible: {Stock}, Solicitado: {quantity}");
        Stock -= quantity;
    }

public void Reserve(int quantity)
    {
        if (quantity <= 0)
            throw new DomainRuleException("La cantidad a reservar debe ser mayor a cero.");

        if (quantity > Stock)
            throw new InsufficientStockException(quantity, Stock);

        Stock -= quantity;
    }
    
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
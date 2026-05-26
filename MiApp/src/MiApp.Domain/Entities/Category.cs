namespace MiApp.Domain.Entities;

public class Category
{
    public Guid Id { get; set; } // Setters públicos para facilitar el seed
    public string Name { get; set; } = string.Empty;
}
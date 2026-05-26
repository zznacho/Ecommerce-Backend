// Features/Products/Commands/CreateProduct/CreateProductCommand.cs
using MediatR;
using FluentValidation;

namespace MiApp.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name, 
    string Description, 
    decimal Price, 
    int Stock) : IRequest<CreateProductResponse>;

// Metemos el validador acá abajo para que herede el namespace perfecto
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThan(0m)
            .WithMessage("El precio debe ser mayor a cero.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El stock no puede ser negativo.");
    }
}
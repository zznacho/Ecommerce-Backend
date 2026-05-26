// Features/Products/Commands/CreateProduct/CreateProductCommand.cs
using MediatR;

namespace MiApp.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name, 
    string Description, 
    decimal Price, 
    int Stock) : IRequest<CreateProductResponse>;
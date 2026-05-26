// Features/Products/Commands/CreateProduct/CreateProductResponse.cs
namespace MiApp.Application.Features.Products.Commands.CreateProduct;

public record CreateProductResponse(Guid Id, string Name, decimal Price, int Stock);
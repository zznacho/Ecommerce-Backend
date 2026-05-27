using MediatR;

namespace MiApp.Application.Features.Products.Queries.GetActiveProducts;

public record GetActiveProductsQuery : IRequest<IReadOnlyList<ProductDto>>;

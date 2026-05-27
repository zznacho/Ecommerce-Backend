using System.Linq;
using MediatR;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Products.Queries.GetActiveProducts;

public class GetActiveProductsQueryHandler : IRequestHandler<GetActiveProductsQuery, IReadOnlyList<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetActiveProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<ProductDto>> Handle(GetActiveProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetActiveProductsAsync(cancellationToken);

        return products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.Stock,
            p.IsActive)).ToList();
    }
}

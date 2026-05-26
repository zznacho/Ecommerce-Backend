// Features/Products/Commands/CreateProduct/CreateProductCommandHandler.cs
using MediatR;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar reglas del negocio contra persistencia (Ej: nombres duplicados)
        if (await _productRepository.ExistsAsync(request.Name, cancellationToken))
        {
            throw new Exception($"Ya existe un producto registrado con el nombre '{request.Name}'.");
        }

        // 2. Crear entidad utilizando el Factory Method del dominio (Valida consistencia interna)
        var product = Product.Create(request.Name, request.Description, request.Price, request.Stock);

        // 3. Persistir mediante las abstracciones
        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 4. Mapear y retornar DTO (Nunca la entidad pura de dominio)
        return new CreateProductResponse(product.Id, product.Name, product.Price, product.Stock);
    }
}
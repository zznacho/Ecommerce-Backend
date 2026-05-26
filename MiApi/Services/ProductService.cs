// Services/ProductService.cs
using MiApi.DTOs;
using MiApi.Models;

namespace MiApi.Services
{
    public class ProductService : IProductService
    {
        // Simulamos nuestra base de datos en una lista estática en memoria
        private static readonly List<Product> _products = new();

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            // Simulamos asincronismo
            await Task.Delay(10);
            
            // Retornamos solo los productos activos mapeados a DTO de respuesta
            return _products.Where(p => p.IsActive).Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            });
        }

        public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
        {
            await Task.Delay(10);
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            
            if (product is null) return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
        {
            await Task.Delay(10);

            // Pasamos los datos del DTO a la Entidad de Dominio real
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _products.Add(newProduct);

            // Devolvemos la respuesta mapeada limpia
            return new ProductResponseDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                Stock = newProduct.Stock
            };
        }

        public async Task<bool> UpdateAsync(Guid id, CreateProductDto dto)
        {
            await Task.Delay(10);
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            
            if (product is null) return false;

            // Actualizamos los campos correspondientes
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await Task.Delay(10);
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            
            if (product is null) return false;

            // Soft Delete: En vez de borrarlo de la lista, lo desactivamos (buena práctica)
            product.IsActive = false;
            return true;
        }
    }
}
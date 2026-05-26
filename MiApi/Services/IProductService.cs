// Services/IProductService.cs
using MiApi.DTOs;

namespace MiApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(Guid id);
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
        Task<bool> UpdateAsync(Guid id, CreateProductDto dto); // Usamos CreateProductDto para actualizar también
        Task<bool> DeleteAsync(Guid id);
    }
}
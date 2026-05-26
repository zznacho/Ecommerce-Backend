// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using MiApi.DTOs;
using MiApi.Services;

namespace MiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // URL base: api/products
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        // Constructor clásico y correcto para Inyección de Dependencias
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products); // 200 OK
        }

        // GET: api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is null) return NotFound(); // 404 Not Found
            
            return Ok(product); // 200 OK
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            // [ApiController] valida de forma automática los campos del DTO (como el [Required])
            var createdProduct = await _productService.CreateAsync(dto);
            
            // 201 Created. Envía la cabecera Location para consultar el nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        // PUT: api/products/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateProductDto dto)
        {
            var updated = await _productService.UpdateAsync(id, dto);
            if (!updated) return NotFound(); // 404 Not Found si el ID no coincide
            
            return NoContent(); // 204 No Content
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _productService.DeleteAsync(id);
            if (!deleted) return NotFound(); // 404 Not Found si no existe
            
            return NoContent(); // 204 No Content
        }
    }
}
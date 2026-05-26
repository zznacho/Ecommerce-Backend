// Controllers/ProductsController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Products.Commands.CreateProduct;

namespace MiApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ISender _mediator;

    public ProductsController(ISender mediator)
    {
        _mediator = mediator;
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        // El ValidationBehavior interceptará el comando y validará las reglas antes de llegar al Handler
        var result = await _mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}
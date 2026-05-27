// Controllers/ProductsController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Products.Commands.CreateProduct;
using MiApp.Application.Features.Products.Queries.GetActiveProducts;

namespace MiApp.WebApi.Controllers;

using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOrUserPolicy")]
public class ProductsController : ControllerBase
{
    private readonly ISender _mediator;

    public ProductsController(ISender mediator)
    {
        _mediator = mediator;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveProductsQuery(), cancellationToken);
        return Ok(result);
    }

    // POST: api/products
    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        // El ValidationBehavior interceptará el comando y validará las reglas antes de llegar al Handler
        var result = await _mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}
using ECommerce.Application.Extenions;
using ECommerce.Application.Features.Products.Create;
using ECommerce.Application.Features.Products.Delete;
using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Features.Products.GetAll;
using ECommerce.Application.Features.Products.GetById;
using ECommerce.Application.Features.Products.Update;
using ECommerce.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

public sealed class ProductController : BaseController
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new ProductGetByIdQuery(id);
        var result = await Mediator.Send(query, cancellationToken);
        return result.Match<ProductDto, IActionResult>(
            product => Ok(product),
            _ => NotFound(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllProductsQuery(paginationParams), cancellationToken);
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result.Match<Guid, IActionResult>(
            product => Ok(product),
            _ => BadRequest(result));
    }

    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command with { Id = id }, cancellationToken);
        return result.Match<Guid, IActionResult>(
            product => Ok(product),
            _ => NotFound(result));
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return result.Match<Guid, IActionResult>(
            product => Ok(product),
            _ => NotFound(result));
    }
}

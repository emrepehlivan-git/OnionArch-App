using ECommerce.Application.Extenions;
using ECommerce.Application.Extensions;
using ECommerce.Application.Features.Orders.Cancel;
using ECommerce.Application.Features.Orders.Create;
using ECommerce.Application.Features.Orders.GetAll;
using ECommerce.Application.Features.Orders.GetById;
using ECommerce.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;
public sealed class OrderController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllOrdersQuery(request), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        return result.Map(
            success => Ok(success),
            error => NotFound(error)
        );
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result.Map(
            success => Ok(success),
            error => BadRequest(error)
        );
    }

    [HttpPut("cancel/{id:guid}")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CancelOrderCommand(id), cancellationToken);
        return Ok(result);
    }
}



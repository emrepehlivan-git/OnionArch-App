using ECommerce.Application.Extensions;
using ECommerce.Application.Features.Categories.Create;
using ECommerce.Application.Features.Categories.Delete;
using ECommerce.Application.Features.Categories.DeleteMany;
using ECommerce.Application.Features.Categories.GetAll;
using ECommerce.Application.Features.Categories.GetById;
using ECommerce.Application.Features.Categories.Update;
using ECommerce.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;
public sealed class CategoryController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllCategoriesQuery(request), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return result.Map(
            success => Ok(result),
            _ => NotFound(result)
        );
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result.Map(
            success => Ok(result),
            _ => BadRequest(result)
        );
    }

    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateCategoryCommand(id, command.Name), cancellationToken);
        return result.Map(
            success => Ok(result),
            _ => BadRequest(result)
        );
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return result.Map(
            success => Ok(result),
            _ => BadRequest(result)
        );
    }

    [HttpDelete("delete-many")]
    public async Task<IActionResult> DeleteMany([FromBody] DeleteManyCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

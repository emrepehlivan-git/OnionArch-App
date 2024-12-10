using ECommerce.Application.Features.Categories.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.GetById;

public record GetByIdQuery(Guid Id) : IRequest<Result<CategoryDto>>;


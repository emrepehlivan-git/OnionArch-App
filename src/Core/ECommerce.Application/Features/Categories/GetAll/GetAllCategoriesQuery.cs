using ECommerce.Application.Features.Categories.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Categories.GetAll;

public record class GetAllCategoriesQuery(PaginationParams Request) : IRequest<PaginatedResult<CategoryDto>>;


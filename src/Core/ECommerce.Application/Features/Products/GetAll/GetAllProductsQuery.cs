using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Products.GetAll;

public sealed record GetAllProductsQuery(PaginationParams PaginationParams) : IRequest<PaginatedResult<ProductDto>>;

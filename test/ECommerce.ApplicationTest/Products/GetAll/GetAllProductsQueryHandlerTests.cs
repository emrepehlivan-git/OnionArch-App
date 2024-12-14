using ECommerce.Application.Features.Products.GetAll;
using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Wrappers;
using ECommerce.Domain.Entities;
using Mapster;


namespace ECommerce.ApplicationTest.Products.GetAll;

public sealed class GetAllProductsQueryHandlerTests : ProductTestBase
{
    private readonly GetAllProductsQueryHandler _handler;
    private readonly PaginationParams _paginationParams;
    private readonly GetAllProductsQuery _query;

    public GetAllProductsQueryHandlerTests()
    {
        _handler = new GetAllProductsQueryHandler(ProductRepositoryMock.Object);
        _paginationParams = new PaginationParams(1, 10);
        _query = new GetAllProductsQuery(_paginationParams);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllProducts_WhenCalled()
    {
        var products = new List<Product>
        {
            Product.Create("Product 1", "Product 1", 100, Guid.NewGuid(), 10),
            Product.Create("Product 2", "Product 2", 200, Guid.NewGuid(), 20),
            Product.Create("Product 3", "Product 3", 300, Guid.NewGuid(), 30)
        };

        ProductRepositoryMock.Setup(repo => repo.GetAllAsync(
            It.IsAny<PaginationParams>(),
            It.IsAny<Expression<Func<Product, bool>>>(),
            It.IsAny<Expression<Func<Product, string>>[]>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()
        ))
            .ReturnsAsync(new PaginatedResult<Product>(products, _paginationParams.PageNumber, _paginationParams.PageSize, products.Count, products.Count));

        var result = await _handler.Handle(_query, CancellationToken.None);
        result.Should().NotBeNull();
        result.Items.Should().BeEquivalentTo(products.Adapt<IEnumerable<ProductDto>>());
        result.PageNumber.Should().Be(_paginationParams.PageNumber);
        result.PageSize.Should().Be(_paginationParams.PageSize);
        result.TotalPages.Should().Be(products.Count);
        result.TotalCount.Should().Be(products.Count);
    }
}

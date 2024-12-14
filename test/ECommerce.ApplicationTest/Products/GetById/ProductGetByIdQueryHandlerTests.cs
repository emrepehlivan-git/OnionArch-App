using ECommerce.Application.Features.Products;
using ECommerce.Application.Features.Products.Dtos;
using ECommerce.Application.Features.Products.GetById;
using Mapster;

namespace ECommerce.ApplicationTest.Products.GetById;

public sealed class ProductGetByIdQueryHandlerTests : ProductTestBase
{
    private readonly ProductGetByIdQueryHandler _handler;

    public ProductGetByIdQueryHandlerTests()
    {
        _handler = new ProductGetByIdQueryHandler(ProductRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenProductIsFound()
    {
        ProductRepositoryMock.Setup(x => x.GetByIdAsync(DefaultProduct.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultProduct);

        var result = await _handler.Handle(new ProductGetByIdQuery(DefaultProduct.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(DefaultProduct.Adapt<ProductDto>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductIsNotFound()
    {
        ProductRepositoryMock.Setup(x => x.GetByIdAsync(DefaultProduct.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Product)null!);

        var result = await _handler.Handle(new ProductGetByIdQuery(DefaultProduct.Id), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ProductErrors.ProductNotFound);
    }
}

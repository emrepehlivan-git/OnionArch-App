using ECommerce.Application.Features.Products;
using ECommerce.Application.Features.Products.Delete;

namespace ECommerce.ApplicationTest.Products.Delete;

public sealed class DeleteProductCommandHandlerTests : ProductTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenProductIsDeleted()
    {
        var command = new DeleteProductCommand(DefaultProduct.Id);
        ProductRepositoryMock.Setup(x => x.GetByIdAsync(DefaultProduct.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultProduct);

        ProductRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new DeleteProductCommandHandler(ProductRepositoryMock.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(DefaultProduct.Id);
        ProductRepositoryMock.Verify(x => x.DeleteAsync(DefaultProduct.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNotFound()
    {
        var productId = Guid.Empty;
        var command = new DeleteProductCommand(productId);

        var validator = new DeleteProductCommandValidator(ProductRepositoryMock.Object);
        var validationResult = await validator.ValidateAsync(command, CancellationToken.None);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNotFound.Message);
    }
}

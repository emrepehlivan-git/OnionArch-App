using ECommerce.Application.Features.Products;
using ECommerce.Application.Features.Products.Update;

namespace ECommerce.ApplicationTest.Products.Update;

public sealed class UpdateProductCommandHandlerTests : ProductTestBase
{
    private readonly UpdateProductCommandValidator _validator;
    private UpdateProductCommand _command = null!;

    public UpdateProductCommandHandlerTests()
    {
        _validator = new UpdateProductCommandValidator(CategoryRepositoryMock.Object, ProductRepositoryMock.Object);
        SetupDefaultProduct();
        _command = new UpdateProductCommand(DefaultProduct.Id, DefaultProduct.Name, DefaultProduct.Description, DefaultProduct.Price, DefaultProduct.CategoryId);
    }


    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenProductIsUpdated()
    {
        ProductRepositoryMock.Setup(x => x.GetByIdAsync(DefaultProduct.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultProduct);

        DefaultProduct.Update(_command.Name, _command.Description, _command.Price, _command.CategoryId);

        ProductRepositoryMock.Setup(x => x.Update(DefaultProduct))
            .Returns(DefaultProduct);

        var handler = new UpdateProductCommandHandler(ProductRepositoryMock.Object);
        var result = await handler.Handle(_command, It.IsAny<CancellationToken>());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(DefaultProduct.Id);
        ProductRepositoryMock.Verify(x => x.Update(DefaultProduct), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNotFound()
    {
        var command = new UpdateProductCommand(Guid.Empty, DefaultProduct.Name, DefaultProduct.Description, DefaultProduct.Price, DefaultProduct.CategoryId);
        var validationResult = await _validator.ValidateAsync(command, It.IsAny<CancellationToken>());

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNotFound.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNameIsTooLong()
    {
        _command = _command with { Name = new string('A', ProductConsts.NameMaxLength + 1) };
        var validationResult = await _validator.ValidateAsync(_command, It.IsAny<CancellationToken>());

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNameMaxLength.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNameIsTooShort()
    {
        _command = _command with { Name = new string(' ', ProductConsts.NameMinLength - 1) };

        var validationResult = await _validator.ValidateAsync(_command, It.IsAny<CancellationToken>());

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNameMinLength.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductPriceIsInvalid()
    {
        _command = _command with { Price = -1 };

        ProductRepositoryMock.Setup(x => x.GetByIdAsync(DefaultProduct.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultProduct);

        var validationResult = await _validator.ValidateAsync(_command, It.IsAny<CancellationToken>());

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductPriceMustBeGreaterThanZero.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenCategoryNotFound()
    {
        var categoryId = Guid.Empty;
        _command = _command with { CategoryId = categoryId };
        ProductRepositoryMock.Setup(x => x.GetByIdAsync(DefaultProduct.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultProduct);

        CategoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Category)null!);

        var validationResult = await _validator.ValidateAsync(_command, It.IsAny<CancellationToken>());

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.CategoryNotFound.Message);
    }
}


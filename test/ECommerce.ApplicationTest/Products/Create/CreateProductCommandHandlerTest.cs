using ECommerce.Application.Features.Products;
using ECommerce.Application.Features.Products.Create;

namespace ECommerce.ApplicationTest.Products.Create;

public class CreateProductCommandHandlerTest : ProductTestBase
{
    private readonly CreateProductCommandHandler _handler;
    private readonly CreateProductCommandValidator _validator;
    private CreateProductCommand _command;

    public CreateProductCommandHandlerTest()
    {
        _handler = new CreateProductCommandHandler(ProductRepositoryMock.Object, CategoryRepositoryMock.Object);
        _validator = new CreateProductCommandValidator(CategoryRepositoryMock.Object);
        _command = new CreateProductCommand(
            DefaultProduct.Name,
            DefaultProduct.Description,
            DefaultProduct.Price,
            DefaultProduct.Stock,
            DefaultCategoryId
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult()
    {
        ProductRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None))
            .Callback<Domain.Entities.Product, CancellationToken>((product, _) =>
            {
                product.GetType().GetProperty("Id")!.SetValue(product, DefaultProduct.Id);
            })
            .Returns((Domain.Entities.Product product, CancellationToken _) => Task.FromResult(product));

        CategoryRepositoryMock.Setup(x => x.GetByIdAsync(DefaultCategoryId, CancellationToken.None))
            .ReturnsAsync(Domain.Entities.Category.Create("Test Category"));

        var result = await _handler.Handle(_command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        ProductRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenCategoryIsNotFound()
    {
        CategoryRepositoryMock.Setup(x => x.GetByIdAsync(DefaultCategoryId, CancellationToken.None))
            .ReturnsAsync((Domain.Entities.Category)null!);

        var result = await _handler.Handle(_command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ProductErrors.CategoryNotFound);
        ProductRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNameIsEmpty()
    {
        var command = _command with { Name = string.Empty };
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNameNotEmpty.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNameIsTooLong()
    {
        var command = _command with { Name = new string('a', ProductConsts.NameMaxLength + 1) };
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNameMaxLength.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNameIsTooShort()
    {
        var command = _command with { Name = string.Empty };
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNameMinLength.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductDescriptionIsTooLong()
    {
        var command = _command with { Description = new string('a', ProductConsts.DescriptionMaxLength + 1) };
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductDescriptionMaxLength.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductPriceIsLessThanZero()
    {
        var command = _command with { Price = -1 };
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductPriceMustBeGreaterThanZero.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenCategoryIdIsInvalid()
    {
        var command = _command with { CategoryId = Guid.Empty };
        var result = await _validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.CategoryNotFound.Message);
    }
}

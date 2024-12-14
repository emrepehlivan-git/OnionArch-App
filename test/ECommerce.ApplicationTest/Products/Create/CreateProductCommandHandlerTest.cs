using ECommerce.Application.Features.Products;
using ECommerce.Application.Features.Products.Create;

namespace ECommerce.ApplicationTest.Products.Create;

public class CreateProductCommandHandlerTest : ProductTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnSuccessResult()
    {
        var command = new CreateProductCommand(
            DefaultProduct.Name,
            DefaultProduct.Description,
            DefaultProduct.Price,
            DefaultProduct.Stock,
            DefaultCategoryId
        );

        ProductRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None))
            .Callback<Domain.Entities.Product, CancellationToken>((product, _) =>
            {
                product.GetType().GetProperty("Id")!.SetValue(product, DefaultProduct.Id);
            })
            .Returns((Domain.Entities.Product product, CancellationToken _) => Task.FromResult(product));

        CategoryRepositoryMock.Setup(x => x.GetByIdAsync(DefaultCategoryId, CancellationToken.None))
            .ReturnsAsync(Domain.Entities.Category.Create("Test Category"));

        var handler = new CreateProductCommandHandler(ProductRepositoryMock.Object, CategoryRepositoryMock.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        ProductRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenCategoryIsNotFound()
    {
        var command = new CreateProductCommand(
            DefaultProduct.Name,
            DefaultProduct.Description,
            DefaultProduct.Price,
            DefaultProduct.Stock,
            DefaultCategoryId
        );

        CategoryRepositoryMock.Setup(x => x.GetByIdAsync(DefaultCategoryId, CancellationToken.None))
            .ReturnsAsync((Domain.Entities.Category)null!);

        var handler = new CreateProductCommandHandler(ProductRepositoryMock.Object, CategoryRepositoryMock.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ProductErrors.CategoryNotFound);
        ProductRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Product>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNameIsEmpty()
    {
        var command = new CreateProductCommand(
            string.Empty,
            DefaultProduct.Description,
            DefaultProduct.Price,
            DefaultProduct.Stock,
            DefaultCategoryId
        );
        var validator = new CreateProductCommandValidator(CategoryRepositoryMock.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNameNotEmpty.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNameIsTooLong()
    {
        var command = new CreateProductCommand(
            new string('a', ProductConsts.NameMaxLength + 1),
            DefaultProduct.Description,
            DefaultProduct.Price,
            DefaultProduct.Stock,
            DefaultCategoryId
        );
        var validator = new CreateProductCommandValidator(CategoryRepositoryMock.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNameMaxLength.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductNameIsTooShort()
    {
        var command = new CreateProductCommand(
            string.Empty,
            DefaultProduct.Description,
            DefaultProduct.Price,
            DefaultProduct.Stock,
            DefaultCategoryId
        );
        var validator = new CreateProductCommandValidator(CategoryRepositoryMock.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductNameMinLength.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductDescriptionIsTooLong()
    {
        var command = new CreateProductCommand(
            DefaultProduct.Name,
            new string('a', ProductConsts.DescriptionMaxLength + 1),
            DefaultProduct.Price,
            DefaultProduct.Stock,
            DefaultCategoryId
        );
        var validator = new CreateProductCommandValidator(CategoryRepositoryMock.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductDescriptionMaxLength.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenProductPriceIsLessThanZero()
    {
        var command = new CreateProductCommand(
            DefaultProduct.Name,
            DefaultProduct.Description,
            -1,
            DefaultProduct.Stock,
            DefaultCategoryId
        );
        var validator = new CreateProductCommandValidator(CategoryRepositoryMock.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.ProductPriceMustBeGreaterThanZero.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenCategoryIdIsInvalid()
    {
        var command = new CreateProductCommand(
            DefaultProduct.Name,
            DefaultProduct.Description,
            DefaultProduct.Price,
            DefaultProduct.Stock,
            Guid.Empty
        );
        var validator = new CreateProductCommandValidator(CategoryRepositoryMock.Object);
        var result = await validator.ValidateAsync(command, CancellationToken.None);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ProductErrors.CategoryNotFound.Message);
    }
}

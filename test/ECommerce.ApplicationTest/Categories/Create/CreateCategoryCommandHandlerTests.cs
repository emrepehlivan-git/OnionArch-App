using ECommerce.Application.Features.Categories;
using ECommerce.Application.Features.Categories.Create;

namespace ECommerce.ApplicationTest.Categories.Create;

public class CreateCategoryCommandHandlerTests : CategoryTestBase
{
    private readonly CreateCategoryCommandHandler _handler;
    private readonly CreateCategoryCommandValidator _validator;
    private readonly CreateCategoryCommand _command;

    public CreateCategoryCommandHandlerTests()
    {
        _handler = new CreateCategoryCommandHandler(CategoryRepositoryMock.Object);
        _validator = new CreateCategoryCommandValidator(CategoryRepositoryMock.Object);
        _command = new CreateCategoryCommand(DefaultCategory.Name);
    }

    [Fact]
    public async Task Handle_ShouldCreateCategory_WhenCommandIsValid()
    {
        CategoryRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Category>(), It.IsAny<CancellationToken>()))
            .Callback<Domain.Entities.Category, CancellationToken>((category, _) =>
            {
                category.GetType().GetProperty("Id")!.SetValue(category, DefaultCategory.Id);
            })
            .Returns((Domain.Entities.Category category, CancellationToken _) => Task.FromResult(category));

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        result.Value.Should().Be(DefaultCategory.Id);
        CategoryRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Domain.Entities.Category>(c => c.Name == _command.Name),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryIsNotAdded()
    {
        // Arrange
        CategoryRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Category)null!);

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(CategoryErrors.CategoryNotAdded);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNameIsAlreadyTaken()
    {
        // Arrange
        CategoryRepositoryMock
            .Setup(x => x.AnyAsync(
                It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.ValidateAsync(_command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryErrors.CategoryNameMustBeUnique.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenNameIsTooLong()
    {
        var result = await _validator.ValidateAsync(_command with { Name = new string('a', CategoryConsts.NameMaxLength + 1) });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryErrors.CategoryNameMaxLength.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenNameIsTooShort()
    {
        var result = await _validator.ValidateAsync(_command with { Name = new string('a', CategoryConsts.NameMinLength - 1) });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryErrors.CategoryNameMinLength.Message);
    }
}

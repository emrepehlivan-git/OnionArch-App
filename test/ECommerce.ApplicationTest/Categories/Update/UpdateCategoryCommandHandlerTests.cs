using ECommerce.Application.Features.Categories;
using ECommerce.Application.Features.Categories.Update;

namespace ECommerce.ApplicationTest.Categories.Update;

public class UpdateCategoryCommandHandlerTests : CategoryTestBase
{
    private readonly UpdateCategoryCommandValidator _validator;
    private readonly UpdateCategoryCommandHandler _handler;
    private readonly UpdateCategoryCommand _command;

    public UpdateCategoryCommandHandlerTests()
    {
        _handler = new UpdateCategoryCommandHandler(CategoryRepositoryMock.Object);
        _validator = new UpdateCategoryCommandValidator(CategoryRepositoryMock.Object);
        _command = new UpdateCategoryCommand(DefaultCategory.Id, "New Name");
    }

    [Fact]
    public async Task Handle_ShouldUpdateCategory_WhenCommandIsValid()
    {
        CategoryRepositoryMock
            .Setup(x => x.GetByIdAsync(_command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultCategory);

        CategoryRepositoryMock
            .Setup(x => x.Update(It.IsAny<Domain.Entities.Category>()))
            .Returns(DefaultCategory);

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(_command.Id);
        CategoryRepositoryMock.Verify(x => x.Update(It.Is<Domain.Entities.Category>(
            c => c.Id == _command.Id && c.Name == _command.Name)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryIsNotUpdated_CategoryNameIsAlreadyTaken()
    {

        CategoryRepositoryMock
            .Setup(x => x.AnyAsync(
                It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validationResult = await _validator.ValidateAsync(_command);
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == CategoryErrors.CategoryNameMustBeUnique.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryIsNotFound()
    {

        CategoryRepositoryMock.Setup(x => x.GetByIdAsync(DefaultCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Category)null!);

        var validationResult = await _validator.ValidateAsync(_command);
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(x => x.ErrorMessage == CategoryErrors.CategoryNotFound.Message);
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
        var result = await _validator.ValidateAsync(_command with { Name = string.Empty });

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == CategoryErrors.CategoryNameMinLength.Message);
    }
}

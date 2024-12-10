using ECommerce.Application.Features.Categories;
using ECommerce.Application.Features.Categories.Delete;

namespace ECommerce.ApplicationTest.Categories.Delete;

public class DeleteCategoryCommandHandlerTests : CategoryTestBase
{
    private readonly DeleteCategoryCommandHandler _handler;
    private readonly DeleteCategoryCommand _command;

    public DeleteCategoryCommandHandlerTests()
    {
        _handler = new DeleteCategoryCommandHandler(CategoryRepositoryMock.Object);
        _command = new DeleteCategoryCommand(DefaultCategory.Id);
    }

    [Fact]
    public async Task Handle_ShouldDeleteCategory_WhenCommandIsValid()
    {
        CategoryRepositoryMock
            .Setup(x => x.DeleteAsync(DefaultCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(DefaultCategory.Id);

        CategoryRepositoryMock.Verify(
            x => x.DeleteAsync(DefaultCategory.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenCategoryDoesNotExist()
    {
        CategoryRepositoryMock
            .Setup(x => x.DeleteAsync(DefaultCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(CategoryErrors.CategoryNotFound);
        result.Value.Should().Be(Guid.Empty);

        CategoryRepositoryMock.Verify(
            x => x.DeleteAsync(DefaultCategory.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
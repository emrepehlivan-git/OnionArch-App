using ECommerce.Application.Features.Categories;
using ECommerce.Application.Features.Categories.Dtos;
using ECommerce.Application.Features.Categories.GetById;

namespace ECommerce.ApplicationTest.Categories.GetById;

public class GetByIdQueryHandlerTests : CategoryTestBase
{
    private readonly GetByIdQueryHandler _handler;
    private readonly GetByIdQuery _query;
    public GetByIdQueryHandlerTests()
    {
        _handler = new GetByIdQueryHandler(CategoryRepositoryMock.Object);
        _query = new GetByIdQuery(DefaultCategory.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategory_WhenCategoryExists()
    {
        CategoryRepositoryMock
            .Setup(x => x.GetByIdAsync(DefaultCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultCategory);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<CategoryDto>();
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(DefaultCategory.Id);
        result.Value.Name.Should().Be(DefaultCategory.Name);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        CategoryRepositoryMock
            .Setup(x => x.GetByIdAsync(DefaultCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Category)null!);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Value.Should().BeNull();
        result.Error.Should().Be(CategoryErrors.CategoryNotFound);
    }
}
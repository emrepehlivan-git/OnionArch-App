using ECommerce.Application.Features.Categories.Dtos;
using ECommerce.Application.Features.Categories.GetAll;
using ECommerce.Application.Wrappers;
using Mapster;

namespace ECommerce.ApplicationTest.Categories.GetAll;

public class GetAllCategoriesQueryHandlerTests : CategoryTestBase
{
    private readonly GetAllCategoriesQueryHandler _handler;
    private readonly GetAllCategoriesQuery _query;
    private readonly PaginationParams _paginationParams;

    public GetAllCategoriesQueryHandlerTests()
    {
        _handler = new GetAllCategoriesQueryHandler(CategoryRepositoryMock.Object);
        _paginationParams = new PaginationParams(1, 10);
        _query = new GetAllCategoriesQuery(_paginationParams);
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedCategories()
    {
        var categories = new List<Domain.Entities.Category>
        {
            Domain.Entities.Category.Create("Category 1"),
            Domain.Entities.Category.Create("Category 2")
        };

        var paginatedResult = new PaginatedResult<Domain.Entities.Category>(
            categories,
            _paginationParams.PageNumber,
            _paginationParams.PageSize,
            categories.Count,
            categories.Count);

        CategoryRepositoryMock
            .Setup(x => x.GetAllAsync(
                _paginationParams,
                It.IsAny<Expression<Func<Domain.Entities.Category, bool>>>(),
                It.IsAny<Expression<Func<Domain.Entities.Category, string>>[]>(),
                false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);
        paginatedResult.Items.Adapt<List<CategoryDto>>();

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(categories.Count);
        result.PageNumber.Should().Be(_paginationParams.PageNumber);
        result.PageSize.Should().Be(_paginationParams.PageSize);
        result.TotalCount.Should().Be(categories.Count);
    }
}
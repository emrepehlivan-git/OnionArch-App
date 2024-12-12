using ECommerce.Application.Interfaces.Repositories;

namespace ECommerce.ApplicationTest.Categories;

public abstract class CategoryTestBase
{
    protected readonly Mock<ICategoryRepository> CategoryRepositoryMock;
    protected Domain.Entities.Category DefaultCategory = null!;

    protected CategoryTestBase()
    {
        CategoryRepositoryMock = new Mock<ICategoryRepository>();
        SetupDefaultCategory();
    }

    protected virtual void SetupDefaultCategory(string name = "Category 1")
    {
        DefaultCategory = Domain.Entities.Category.Create(name);
    }
}

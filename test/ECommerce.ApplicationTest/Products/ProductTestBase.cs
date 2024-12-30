using ECommerce.Application.Interfaces.Repositories;

namespace ECommerce.ApplicationTest.Products;

public abstract class ProductTestBase
{
    protected readonly Mock<IProductRepository> ProductRepositoryMock;
    protected readonly Mock<ICategoryRepository> CategoryRepositoryMock;
    protected readonly Guid DefaultCategoryId = new("3559cceb-c88e-46c6-8a2a-fafa085254bb");
    protected Domain.Entities.Product DefaultProduct = null!;

    protected ProductTestBase()
    {
        ProductRepositoryMock = new Mock<IProductRepository>();
        CategoryRepositoryMock = new Mock<ICategoryRepository>();
        DefaultProduct = Domain.Entities.Product.Create(
            "Product 1",
            "Description 1",
            100,
            DefaultCategoryId,
            10);
    }
}

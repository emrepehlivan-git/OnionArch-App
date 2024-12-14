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
        SetupDefaultProduct();
    }

    protected virtual void SetupDefaultProduct(
        string name = "Product 1",
        string description = "Description 1",
        decimal price = 100,
        Guid? categoryId = null,
        int stock = 10)
    {
        DefaultProduct = Domain.Entities.Product.Create(
            name,
            description,
            price,
            categoryId ?? DefaultCategoryId,
            stock);
    }

}

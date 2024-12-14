using ECommerce.Application.Interfaces.Repositories;

namespace ECommerce.ApplicationTest.Orders;

public abstract class OrderTestBase
{
    protected readonly Mock<IOrderRepository> OrderRepositoryMock;
    protected readonly Mock<IProductRepository> ProductRepositoryMock;
    protected readonly Mock<IOrderItemRepository> OrderItemRepositoryMock;
    protected OrderTestBase()
    {
        OrderRepositoryMock = new Mock<IOrderRepository>();
        ProductRepositoryMock = new Mock<IProductRepository>();
        OrderItemRepositoryMock = new Mock<IOrderItemRepository>();
    }
}

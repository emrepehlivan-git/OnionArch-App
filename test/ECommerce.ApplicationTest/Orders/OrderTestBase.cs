using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.ApplicationTest.Orders;

public abstract class OrderTestBase
{
    protected readonly Mock<IOrderRepository> OrderRepositoryMock;
    protected readonly Mock<IProductRepository> ProductRepositoryMock;
    protected readonly Mock<IOrderItemRepository> OrderItemRepositoryMock;
    protected readonly Order DefaultOrder;
    protected OrderTestBase()
    {
        OrderRepositoryMock = new Mock<IOrderRepository>();
        ProductRepositoryMock = new Mock<IProductRepository>();
        OrderItemRepositoryMock = new Mock<IOrderItemRepository>();
        DefaultOrder = Order.Create(PaymentMethod.CreditCard, new Address("Street", "City", "State", "Country", "ZipCode"));
    }
}

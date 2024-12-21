using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.ValueObjects;
using MediatR;

namespace ECommerce.ApplicationTest.Orders;

public abstract class OrderTestBase
{
    protected readonly Mock<IOrderRepository> OrderRepositoryMock;
    protected readonly Mock<IProductRepository> ProductRepositoryMock;
    protected readonly Mock<IOrderItemRepository> OrderItemRepositoryMock;
    protected readonly Mock<IStockService> StockServiceMock;
    protected readonly Mock<IMediator> MediatorMock;
    protected readonly Order DefaultOrder;
    protected OrderTestBase()
    {
        OrderRepositoryMock = new();
        OrderItemRepositoryMock = new();
        ProductRepositoryMock = new();
        StockServiceMock = new();
        MediatorMock = new();
        DefaultOrder = Order.Create(PaymentMethod.CreditCard, new Address("Street", "City", "State", "Country", "ZipCode"));
    }
}

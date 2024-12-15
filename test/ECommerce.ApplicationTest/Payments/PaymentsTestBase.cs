using System;
using ECommerce.Application.Dtos.Payment;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;

namespace ECommerce.ApplicationTest.Payments;

public abstract class PaymentsTestBase
{
    protected readonly Mock<IPaymentService> PaymentServiceMock;
    protected readonly Mock<IOrderRepository> OrderRepositoryMock;
    protected readonly Guid OrderId;
    protected readonly PaymentRequest PaymentRequest;

    protected PaymentsTestBase()
    {
        PaymentServiceMock = new();
        OrderRepositoryMock = new();
        OrderId = Guid.NewGuid();
        PaymentRequest = new(OrderId, "1234567890", "John Doe", "12/25", "123", 100);
    }
}
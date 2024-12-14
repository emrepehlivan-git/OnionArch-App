using ECommerce.Application.Dtos.Payment;
using ECommerce.Application.Wrappers;

namespace ECommerce.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<Result<PaymentResponse>> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default);
}

using ECommerce.Application.Dtos.Payment;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Wrappers;

namespace ECommerce.Infrastructure.Services;

public sealed class PaymentService : IPaymentService
{
    public Task<Result<PaymentResponse>> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var random = new Random();
        var isSuccess = random.Next(0, 2) == 0;
        return Task.FromResult(Result<PaymentResponse>.Success(new PaymentResponse(isSuccess, $"Payment processed {isSuccess}")));
    }
}

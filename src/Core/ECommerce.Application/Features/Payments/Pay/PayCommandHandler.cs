using ECommerce.Application.Dtos.Payment;
using ECommerce.Application.Features.Payments.Pay;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Payments;

public sealed class PayCommandHandler(IPaymentService paymentService) : IRequestHandler<PayCommand, Result<PaymentResponse>>
{
    public async Task<Result<PaymentResponse>> Handle(PayCommand request, CancellationToken cancellationToken)
    {
        var result = await paymentService.ProcessPaymentAsync(request.PaymentRequest, cancellationToken);

        return result.IsSuccess ? Result<PaymentResponse>.Success(result.Value!) : Result<PaymentResponse>.Failure(result.Error);
    }
}

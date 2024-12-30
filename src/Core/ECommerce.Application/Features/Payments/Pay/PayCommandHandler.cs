using ECommerce.Application.Features.Payments.Pay;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Payments;

public sealed class PayCommandHandler(IPaymentService paymentService) : IRequestHandler<PayCommand, Result>
{
    public async Task<Result> Handle(PayCommand request, CancellationToken cancellationToken)
    {
        var result = await paymentService.ProcessPaymentAsync(request.PaymentRequest, cancellationToken);

        return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
    }
}

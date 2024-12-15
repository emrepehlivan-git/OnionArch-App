using ECommerce.Application.Dtos.Payment;
using ECommerce.Application.Wrappers;
using MediatR;

namespace ECommerce.Application.Features.Payments.Pay;

public record PayCommand(PaymentRequest PaymentRequest) : IRequest<Result<PaymentResponse>>;

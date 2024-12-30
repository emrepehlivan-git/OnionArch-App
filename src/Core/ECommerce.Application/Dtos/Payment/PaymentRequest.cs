namespace ECommerce.Application.Dtos.Payment;

public record PaymentRequest(Guid OrderId, string CardNumber, string CardHolderName, string ExpiryDate, string Cvv, decimal Amount);

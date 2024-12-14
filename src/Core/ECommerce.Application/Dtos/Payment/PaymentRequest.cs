namespace ECommerce.Application.Dtos.Payment;

public record PaymentRequest(string CardNumber, string ExpiryDate, string Cvv, decimal Amount);

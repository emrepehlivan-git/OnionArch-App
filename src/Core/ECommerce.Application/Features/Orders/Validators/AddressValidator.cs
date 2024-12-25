using ECommerce.Domain.ValueObjects;
using FluentValidation;

namespace ECommerce.Application.Features.Orders.Validators;

public sealed class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage(OrderErrors.InvalidAddress.Message);

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage(OrderErrors.InvalidAddress.Message);

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage(OrderErrors.InvalidAddress.Message);

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage(OrderErrors.InvalidAddress.Message);

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage(OrderErrors.InvalidAddress.Message);
    }
}

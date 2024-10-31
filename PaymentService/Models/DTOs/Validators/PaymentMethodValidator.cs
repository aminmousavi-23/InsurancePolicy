using FluentValidation;

namespace PaymentService.Models.DTOs.Validators;

public class PaymentMethodValidator : AbstractValidator<PaymentMethodDto>
{
    public PaymentMethodValidator()
    {
        RuleFor(pm => pm.Name)
            .NotEmpty().WithMessage("Please enter the name of payment method")
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(25);

        RuleFor(pm => pm.Description)
            .NotEmpty().WithMessage("Please enter payment method description")
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(250);
    }
}

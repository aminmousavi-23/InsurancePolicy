using FluentValidation;

namespace PaymentService.Models.DTOs.Validators;

public class PaymentValidator : AbstractValidator<PaymentDto>
{
    public PaymentValidator()
    {
        RuleFor(p => p.PaymentDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Payment action time can not be greater than today");

        RuleFor(p => p.Description)
        .MinimumLength(5)
        .MaximumLength(250)
        .NotEmpty().WithMessage("Please enter Description")
        .NotNull();
    }
}

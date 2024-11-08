using FluentValidation;

namespace PaymentService.Models.DTOs.Validators;

public class RefundValidator : AbstractValidator<RefundDto>
{
    public RefundValidator()
    {
        RuleFor(r => r.Amount)
            .NotEmpty().WithMessage("Please enter the Amount of refund")
            .NotNull();

        RuleFor(r => r.RefundDate)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Refund Date can not be greater than today");

        RuleFor(r => r.Reason)
            .MinimumLength(5)
            .MaximumLength(250)
            .NotEmpty().WithMessage("Please enter the Refundation Reason")
            .NotNull();
    }
}

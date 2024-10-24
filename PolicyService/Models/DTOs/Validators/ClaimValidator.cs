using FluentValidation;

namespace PolicyService.Models.DTOs.Validators;

public class ClaimValidator : AbstractValidator<ClaimDto>
{
    public ClaimValidator()
    {
        RuleFor(c => c.Description)
            .MinimumLength(5)
            .MaximumLength(250)
            .NotEmpty().WithMessage("Please enter Description")
            .NotNull();

        RuleFor(c => c.ClaimAmount)
            .NotEmpty().WithMessage("Please enter Claim Amount")
            .NotNull();
    }
}

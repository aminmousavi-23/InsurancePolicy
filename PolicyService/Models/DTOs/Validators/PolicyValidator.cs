using FluentValidation;

namespace PolicyService.Models.DTOs.Validators;

public class PolicyValidator : AbstractValidator<PolicyDto>
{
    public PolicyValidator()
    {
        RuleFor(p => p.StartDate)
            .NotEmpty().WithMessage("Please Enter the Policy Start Date")
            .NotNull();

        RuleFor(p => p.EndDate)
            .GreaterThan(p => p.StartDate)
            .NotEmpty().WithMessage("Please Enter the Policy End Date")
            .NotNull();

        RuleFor(p => p.PremiumAmount)
            .NotEmpty()
            .NotNull();

        RuleFor(p => p.Status)
            .NotEmpty().WithMessage("Please Select the Policy Status")
            .NotNull();
    }
}

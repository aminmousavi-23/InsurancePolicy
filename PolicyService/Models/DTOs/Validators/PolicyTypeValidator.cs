using FluentValidation;

namespace PolicyService.Models.DTOs.Validators;

public class PolicyTypeValidator : AbstractValidator<PolicyTypeDto>
{
    public PolicyTypeValidator()
    {
        RuleFor(pt => pt.Name)
            .MaximumLength(25)
            .NotEmpty().WithMessage("Please enter Policy Type Name")
            .NotNull();

        RuleFor(pt => pt.Description)
            .MinimumLength(5)
            .MaximumLength(250)
            .NotEmpty().WithMessage("Please enter Policy Type Description")
            .NotNull();
    }
}

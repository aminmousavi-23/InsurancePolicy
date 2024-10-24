using FluentValidation;

namespace PolicyService.Models.DTOs.Validators;

public class CoverageValidator : AbstractValidator<CoverageDto>
{
    public CoverageValidator()
    {
        RuleFor(c => c.CoverageName)
            .MaximumLength(25)
            .NotEmpty().WithMessage("Please enter Coverage Name")
            .NotNull();

        RuleFor(c => c.Description)
            .MinimumLength(5)
            .MaximumLength(250)
            .NotEmpty().WithMessage("Please enter Description")
            .NotNull();

        RuleFor(c => c.CoverageAmount)
            .NotEmpty().WithMessage("Please enter Coverage Amount")
            .NotNull();
    }
}

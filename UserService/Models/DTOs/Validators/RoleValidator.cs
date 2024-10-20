using FluentValidation;


namespace UserService.Models.DTOs.Validators;

public class RoleValidator : AbstractValidator<RoleDto>
{
    public RoleValidator()
    {
        RuleFor(role => role.RoleName)
            .MinimumLength(3)
            .MaximumLength(25)
            .NotNull()
            .NotEmpty().WithMessage("Please Enter Role Name !!!");
    }
}

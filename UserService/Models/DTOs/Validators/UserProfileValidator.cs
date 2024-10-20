using FluentValidation;

namespace UserService.Models.DTOs.Validators
{
    public class UserProfileValidator : AbstractValidator<UserProfileDto>
    {
        public UserProfileValidator()
        {
            RuleFor(up => up.Address)
                .MaximumLength(100)
                .NotEmpty().WithMessage("Please Enter your Address")
                .NotNull();

            RuleFor(up => up.City)
                .MaximumLength(25)
                .NotEmpty().WithMessage("Please Enter your City")
                .NotNull();

            RuleFor(up => up.State)
                .MaximumLength(25)
                .NotEmpty().WithMessage("Please Enter your State")
                .NotNull();

            RuleFor(up => up.Country)
                .MaximumLength(25)
                .NotEmpty().WithMessage("Please Enter your Country")
                .NotNull();

            RuleFor(up => up.PostalCode)
                .MaximumLength(15)
                .NotEmpty().WithMessage("Please Enter your Postal Code")
                .NotNull();
        }
    }
}

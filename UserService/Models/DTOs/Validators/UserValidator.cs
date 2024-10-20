﻿using FluentValidation;

namespace UserService.Models.DTOs.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user.FirstName)
                .MinimumLength(3)
                .MaximumLength(25)
                .NotEmpty().WithMessage("Please Enter your First Name")
                .NotNull();

            RuleFor(user => user.LastName)
                .MinimumLength(3)
                .MaximumLength(25)
                .NotEmpty().WithMessage("Please Enter your Last Name")
                .NotNull();

            RuleFor(user => user.Email)
                .EmailAddress()
                .NotEmpty().WithMessage("Please Enter your Email")
                .NotNull();

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Please Enter your Password")
                .NotNull();

            RuleFor(user => user.ConfirmPassword)
                .Equal(user => user.Password).WithMessage("Password and ConfirmPassword must be the same")
                .NotEmpty().WithMessage("Please re-enter your Password")
                .NotNull();

            RuleFor(user => user.PhoneNumber)
                .Length(11)
                .NotEmpty().WithMessage("Please Enter your Phone Number")
                .NotNull();
        }
    }
}
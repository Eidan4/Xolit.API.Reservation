using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Xolit.API.Reservation.Application.DTOs.Space;

namespace Xolit.API.Reservation.Application.DTOs.User.Validation
{
    public class CreateUserValidator : AbstractValidator<UserDto>
    {
        public CreateUserValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required");
        }
    }
}

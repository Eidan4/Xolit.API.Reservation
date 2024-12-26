using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Xolit.API.Reservation.Application.DTOs.Space;

namespace Xolit.API.Reservation.Application.DTOs.User.Validation
{
    public class UpdateUserValidator : AbstractValidator<UserDto>
    {
        public UpdateUserValidator() {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}

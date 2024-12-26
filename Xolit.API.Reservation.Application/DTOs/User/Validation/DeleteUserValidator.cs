using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Xolit.API.Reservation.Application.DTOs.Space;
using Xolit.API.Reservation.Application.Features.User.Request.Commands;

namespace Xolit.API.Reservation.Application.DTOs.User.Validation
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator() {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}

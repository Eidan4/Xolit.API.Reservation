using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Xolit.API.Reservation.Application.Features.Space.Request.Commands;

namespace Xolit.API.Reservation.Application.DTOs.Space.Validation
{
    public class DeleteSpaceValidator : AbstractValidator<DeleteSpaceCommand>
    {
        public DeleteSpaceValidator() {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}

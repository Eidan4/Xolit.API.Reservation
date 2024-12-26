using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;

namespace Xolit.API.Reservation.Application.DTOs.Recervation.Validation
{
    public class DeleteReservationValidator : AbstractValidator<DeleteReservationCommand>
    {
        public DeleteReservationValidator() {

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

        }
    }
}

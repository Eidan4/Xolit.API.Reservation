using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Xolit.API.Reservation.Application.DTOs.Recervation.Validation
{
    public class UpdateReservationValidator : AbstractValidator<ReservationDto>
    {
        public UpdateReservationValidator()
        {

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.SpaceId)
                .NotEmpty().WithMessage("SpaceId is required");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("StartTime is required");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("EndTime is required");
        }
    }
}

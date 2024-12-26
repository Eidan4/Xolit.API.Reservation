using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Xolit.API.Reservation.Application.DTOs.Recervation;

namespace Xolit.API.Reservation.Application.DTOs.Space.Validation
{
    public class CreateSpaceValidator : AbstractValidator<SpaceDto>
    {
        public CreateSpaceValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("StartTime is required");
        }
    }
}

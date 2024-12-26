using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Xolit.API.Reservation.Application.DTOs.Space.Validation
{
    public class UpdateSpaceValidator : AbstractValidator<SpaceDto>
    {
        public UpdateSpaceValidator() {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.Capacity)
                .NotEmpty().WithMessage("StartTime is required");
        }
    }
}

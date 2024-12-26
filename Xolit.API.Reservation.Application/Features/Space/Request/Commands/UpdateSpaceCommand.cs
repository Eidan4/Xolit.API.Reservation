using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Xolit.API.Reservation.Application.DTOs.Space;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.Space.Request.Commands
{
    public class UpdateSpaceCommand : IRequest<BaseCommandResponse>
    {
        public SpaceDto SpaceDto { get; set; }
    }
}

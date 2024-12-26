using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Xolit.API.Reservation.Application.DTOs.Recervation;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.Reservation.Request.Commands
{
    public class CreateReservationCommand : IRequest<BaseCommandResponse>
    {
        public ReservationDto ReservationDto { get; set; }
    }
}

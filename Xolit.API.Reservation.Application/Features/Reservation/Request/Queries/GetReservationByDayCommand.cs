using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.Reservation.Request.Queries
{
    public class GetReservationByDayCommand : IRequest<BaseCommandResponse>
    {
        public string Day { get; set; }
        public int SpaceId { get; set; }
    }
}

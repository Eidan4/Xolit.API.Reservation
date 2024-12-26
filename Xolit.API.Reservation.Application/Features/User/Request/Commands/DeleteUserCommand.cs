using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Application.Features.User.Request.Commands
{
    public class DeleteUserCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
    }
}

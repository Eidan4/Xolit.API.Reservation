using MediatR;
using Microsoft.AspNetCore.Mvc;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Queries;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("CreateReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> CreateReservation([FromBody] CreateReservationCommand createReservationCommand)
        {
            var response = await _mediator.Send(createReservationCommand);

            return Ok(response);
        }

        [HttpPut("UpdateReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateReservation([FromBody] UpdateReservationCommand updateReservationCommand)
        {
            var response = await _mediator.Send(updateReservationCommand);

            return Ok(response);
        }

        [HttpDelete("DeleteReservation/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> DeleteReservation(int id)
        {
            var response = await _mediator.Send(new DeleteReservationCommand { Id = id });

            return Ok(response);
        }

        [HttpGet("GetAvailableHouser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetAvailableHouser(string day, int spaceId)
        {
            var response = await _mediator.Send(new GetReservationByDayCommand { Day = day, SpaceId = spaceId });

            return Ok(response);
        }

        [HttpGet("GetByUserIdReservation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> GetByUserIdReservation(int userId)
        {
            var response = await _mediator.Send(new GetByUserIdReservationCommand { UserId = userId });

            return Ok(response);
        }
    }
}

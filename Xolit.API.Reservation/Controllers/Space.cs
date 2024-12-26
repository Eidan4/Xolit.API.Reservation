using MediatR;
using Microsoft.AspNetCore.Mvc;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Features.Space.Request.Commands;
using Xolit.API.Reservation.Application.Features.User.Request.Commands;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpaceController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("CreateSpace")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> CreateUser([FromBody] CreateSpaceCommand createSpaceCommand)
        {
            var response = await _mediator.Send(createSpaceCommand);

            return Ok(response);
        }

        [HttpPut("UpdateSpace")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateUser([FromBody] UpdateSpaceCommand updateSpaceCommand)
        {
            var response = await _mediator.Send(updateSpaceCommand);

            return Ok(response);
        }

        [HttpDelete("DeleteSpace/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> DeleteUser(int id)
        {
            var response = await _mediator.Send(new DeleteSpaceCommand { Id = id });

            return Ok(response);
        }
    }
}

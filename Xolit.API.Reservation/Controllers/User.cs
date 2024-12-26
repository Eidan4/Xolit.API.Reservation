using MediatR;
using Microsoft.AspNetCore.Mvc;
using Xolit.API.Reservation.Application.Features.Reservation.Request.Commands;
using Xolit.API.Reservation.Application.Features.User.Request.Commands;
using Xolit.API.Reservation.Application.Response;

namespace Xolit.API.Reservation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> CreateUser([FromBody] CreateUserCommand createUserCommand)
        {
            var response = await _mediator.Send(createUserCommand);

            return Ok(response);
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> LoginUser([FromBody] LoginUserCommand loginUserCommand)
        {
            var response = await _mediator.Send(loginUserCommand);

            return Ok(response);
        }

        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> UpdateUser([FromBody] UpdateUserCommand updateUserCommand)
        {
            var response = await _mediator.Send(updateUserCommand);

            return Ok(response);
        }

        [HttpDelete("DeleteUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseCommandResponse>> DeleteUser(int id)
        {
            var response = await _mediator.Send(new DeleteUserCommand { Id = id });

            return Ok(response);
        }
    }
}

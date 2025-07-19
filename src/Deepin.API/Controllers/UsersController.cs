using Deepin.API.Models.Users;
using Deepin.Application.Commands.Users;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Users;
using Deepin.Application.Queries;
using Deepin.Application.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.API.Controllers
{
    public class UsersController(IMediator mediator, IUserQueries userQueries) : ApiControllerBase
    {
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await mediator.Send(new GetUserCommand(id), cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost("batch")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> BatchGetUsers([FromBody] BatchGetUserRequest request, CancellationToken cancellationToken = default)
        {
            var users = await userQueries.GetUsersAsync(request.Ids, cancellationToken);
            return Ok(users);
        }
        [HttpGet("search")]
        [ProducesResponseType(typeof(IPagedResult<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IPagedResult<UserDto>>> Search([FromQuery] SearchUserRequest request, CancellationToken cancellationToken = default)
        {
            var result = await userQueries.SearchUsersAsync(request.Limit, request.Offset, request.Search, cancellationToken);
            return Ok(result);
        }
        [HttpGet("identity/{identity}")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUserByIdentity(string identity, CancellationToken cancellationToken = default)
        {
            var user = await userQueries.GetUserByIdentityAsync(identity, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost("{id:guid}/claims")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UpdateUserClaims(Guid id, [FromBody] IEnumerable<UserCliamRequest> requests, CancellationToken cancellationToken = default)
        {
            if (requests == null || !requests.Any())
            {
                return BadRequest("Claims cannot be null or empty.");
            }
            var user = await mediator.Send(new GetUserCommand(id), cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            var command = new UpdateUserCliamsCommand(id, requests);
            var updatedUser = await mediator.Send(command, cancellationToken);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }
    }
}

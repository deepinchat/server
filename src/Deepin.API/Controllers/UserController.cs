using Deepin.API.Models.Users;
using Deepin.Application.Commands.Users;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Users;
using Deepin.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.API.Controllers
{
    public class UserController(IMediator mediator, IUserQueries userQueries) : ApiControllerBase
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
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromBody] Guid[] ids, CancellationToken cancellationToken = default)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No user IDs provided.");
            }
            var users = await userQueries.GetUsersAsync(ids, cancellationToken);
            if (users == null || !users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }
        [HttpGet("search")]
        [ProducesResponseType(typeof(IPagedResult<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IPagedResult<UserDto>>> SearchUsers([FromQuery] SearchUserRequest request, CancellationToken cancellationToken = default)
        {
            var result = await userQueries.SearchUsersAsync(request.Limit, request.Offset, request.Search, cancellationToken);
            return Ok(result);
        }
    }
}

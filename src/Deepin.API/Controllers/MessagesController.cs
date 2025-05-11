using Deepin.API.Models.Messages;
using Deepin.API.Services;
using Deepin.Application.Commands.Messages;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Messages;
using Deepin.Application.Requests.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController(IMediator mediator, IChatService chatService) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MessageSummary>> Get(Guid id, CancellationToken cancellationToken)
        {
            var message = await chatService.GetMessage(id, cancellationToken);
            if (message is null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> Post([FromBody] SendMessageRequest request, CancellationToken cancellationToken)
        {
            var message = await _mediator.Send(new SendMessageCommand(request), cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = message.Id }, message);
        }
        [HttpGet]
        public async Task<ActionResult<IPagedResult<MessageDto>>> SearchMessages([FromQuery] SearchMessageRequest request, CancellationToken cancellationToken)
        {
            var result = await chatService.SearchMessages(request, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}

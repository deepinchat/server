using Deepin.API.Models.Messages;
using Deepin.Application.Commands.Messages;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Messages;
using Deepin.Application.Queries;
using Deepin.Application.Requests.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.API.Controllers
{
    public class MessagesController(
        IMediator mediator,
        IMessageQueries messageQueries) : ApiControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MessageDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var message = await mediator.Send(new GetMessageCommand(id), cancellationToken);
            if (message is null)
            {
                return NotFound();
            }
            return Ok(message);
        }
        [HttpGet]
        public async Task<ActionResult<IPagedResult<MessageDto>>> GetMessages([FromQuery] Guid[] ids, CancellationToken cancellationToken = default)
        {
            if (ids is null || ids.Length == 0)
            {
                return BadRequest("No message IDs provided.");
            }
            var result = await messageQueries.GetMessagesAsync(ids, cancellationToken: cancellationToken);
            return Ok(result);
        }
        [HttpGet("search")]
        public async Task<ActionResult<IPagedResult<MessageDto>>> SearchMessages([FromQuery] SearchMessageRequest request, CancellationToken cancellationToken)
        {
            var result = await messageQueries.SearchMessagesAsync(
                limit: request.Limit,
                offset: request.Offset,
                search: request.Search,
                chatId: request.ChatId,
                userId: request.UserId,
                cancellationToken: cancellationToken);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<MessageDto>> Send([FromBody] SendMessageRequest request, CancellationToken cancellationToken)
        {
            var message = await mediator.Send(new SendMessageCommand(request), cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = message.Id }, message);
        }
        [HttpGet("lasts")]
        public async Task<ActionResult<IEnumerable<LastMessageDto>>> GetLastMessages([FromQuery] Guid[] chatIds, CancellationToken cancellationToken)
        {
            if (chatIds is null || chatIds.Length == 0)
            {
                return BadRequest("No chat IDs provided.");
            }
            var lastMessages = await messageQueries.GetLastMessageIdsAsync(chatIds, cancellationToken);
            return Ok(lastMessages);
        }
        [HttpGet("unread-count/{chatId:guid}")]
        public async Task<IActionResult> GetUnreadCount([FromQuery] Guid chatId, [FromQuery] DateTimeOffset? lastReadAt = null, CancellationToken cancellationToken = default)
        {
            var count = await messageQueries.GetUnreadCountAsync(chatId, lastReadAt, cancellationToken);
            if (count < 0)
            {
                return NotFound();
            }
            return Ok(new
            {
                ChatId = chatId,
                UnreadCount = count
            });
        }
    }
}

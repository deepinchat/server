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
        [HttpPost("batch")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesByIds([FromBody] Guid[] ids, CancellationToken cancellationToken = default)
        {
            if (ids is null || ids.Length == 0)
            {
                return BadRequest("No message IDs provided.");
            }
            var result = await messageQueries.GetMessagesAsync(ids, cancellationToken: cancellationToken);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<IPagedResult<MessageDto>>> GetMessages([FromQuery] SearchMessageRequest request, CancellationToken cancellationToken)
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
        [HttpPost("lasts")]
        public async Task<ActionResult<IEnumerable<LastMessageDto>>> GetLastMessages([FromBody] GetLastMessagesRequest request, CancellationToken cancellationToken)
        {
            if (request is null || request.ChatIds is null || request.ChatIds.Count() == 0)
            {
                return BadRequest("No chat IDs provided.");
            }
            var lastMessages = await messageQueries.GetLastMessageIdsAsync(request.ChatIds.ToArray(), cancellationToken);
            return Ok(lastMessages);
        }
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount([FromQuery] GetUnreadMessageCountRequest request, CancellationToken cancellationToken = default)
        {
            var count = await messageQueries.GetUnreadCountAsync(request.ChatId, request.LastReadAt, cancellationToken);
            if (count < 0)
            {
                return NotFound();
            }
            return Ok(new
            {
                UnreadCount = count,
                request.ChatId,
                request.LastReadAt
            });
        }
    }
}

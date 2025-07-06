using Deepin.API.Models.Messages;
using Deepin.Application.Commands.Messages;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Messages;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Application.Requests.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.API.Controllers
{
    public class MessagesController(
        IMediator mediator,
        IUserContext userContext,
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
        public async Task<ActionResult<IEnumerable<MessageDto>>> BatchGetMessages([FromBody] BatchGetMessageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await messageQueries.GetMessagesAsync(request.Ids, cancellationToken: cancellationToken);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<IPagedResult<MessageDto>>> Search([FromQuery] SearchMessageRequest request, CancellationToken cancellationToken)
        {
            var result = await messageQueries.SearchMessagesAsync(
                limit: request.Limit,
                offset: request.Offset,
                search: request.Search,
                chatId: request.ChatId,
                userId: request.UserId,
                sortBy: request.SortBy ?? SortDirection.Descending,
                readAt: request.ReadAt,
                cancellationToken: cancellationToken);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<MessageDto>> Send([FromBody] MessageRequest request, CancellationToken cancellationToken)
        {
            var message = await mediator.Send(new SendMessageCommand(request, userContext.UserId), cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = message.Id }, message);
        }
        [HttpPost("lasts")]
        public async Task<ActionResult<IEnumerable<LastMessageDto>>> GetLasts([FromBody] GetLastMessagesRequest request, CancellationToken cancellationToken)
        {
            if (request is null || request.ChatIds is null || request.ChatIds.Count() == 0)
            {
                return BadRequest("No chat IDs provided.");
            }
            var lastMessages = await messageQueries.GetLastMessageIdsAsync(request.ChatIds.ToArray(), cancellationToken);
            return Ok(lastMessages);
        }
    }
}

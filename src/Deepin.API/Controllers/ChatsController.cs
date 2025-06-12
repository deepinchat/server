using Deepin.API.Models.Chats;
using Deepin.Application.Commands.Chats;
using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Chats;
using Deepin.Application.Interfaces;
using Deepin.Application.Queries;
using Deepin.Domain.ChatAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deepin.API.Controllers
{
    public class ChatsController(IMediator mediator, IChatQueries chatQueries, IUserContext userContext) : ApiControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IChatQueries _chatQueries = chatQueries;
        private readonly IUserContext _userContext = userContext;

        [HttpGet("{id}")]
        public async Task<ActionResult<ChatDto>> Get(Guid id, CancellationToken cancellationToken = default)
        {
            var chat = await _mediator.Send(new GetChatCommand(id), cancellationToken);
            if (chat == null)
                return NotFound();
            return Ok(chat);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatDto>>> GetChats(CancellationToken cancellationToken = default)
        {
            var chats = await _mediator.Send(new GetChatsCommand(_userContext.UserId), cancellationToken);
            return Ok(chats);
        }
        [HttpGet("direct")]
        public async Task<ActionResult<IEnumerable<ChatDto>>> GetDirectChats(CancellationToken cancellationToken = default)
        {
            var chats = await _mediator.Send(new GetDirectChatsCommand(_userContext.UserId), cancellationToken);
            return Ok(chats);
        }
        [HttpGet("search")]
        public async Task<ActionResult<IPagedResult<ChatDto>>> SearchChats([FromQuery] SearchChatRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _chatQueries.SearchChats(
                limit: request.Limit,
                offset: request.Offset,
                search: request.Search,
                type: request.Type,
                cancellationToken: cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ChatDto>> Update(Guid id, [FromBody] UpdateChatRequest request, CancellationToken cancellationToken = default)
        {
            var chat = await _mediator.Send(new UpdateChatCommand()
            {
                Id = id,
                Name = request.Name,
                UserName = request.UserName,
                Description = request.Description,
                AvatarFileId = request.AvatarFileId,
                IsPublic = request.IsPublic
            }, cancellationToken);
            return Ok(chat);
        }
        [HttpPost]
        public async Task<ActionResult<ChatDto>> Create([FromBody] CreateChatRequest request, CancellationToken cancellationToken = default)
        {
            var chat = await _mediator.Send(new CreateChatCommand
            {
                OwnerId = _userContext.UserId,
                Name = request.Name,
                UserName = request.UserName,
                Description = request.Description,
                AvatarFileId = request.AvatarFileId,
                IsPublic = request.IsPublic,
                Type = request.Type
            }, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = chat.Id }, chat);
        }
        [HttpPost("direct")]
        public async Task<ActionResult<ChatDto>> CreateDirectChat([FromBody] Guid[] users, CancellationToken cancellationToken = default)
        {
            var chat = await _mediator.Send(new CreateDirectChatCommand(_userContext.UserId, Others: users), cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = chat.Id }, chat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var chatMember = await _chatQueries.GetChatMember(chatId: id, _userContext.UserId, cancellationToken);
            if (chatMember == null)
                return NotFound();
            if (chatMember.Role == ChatMemberRole.Owner)
                return Forbid();
            await _mediator.Send(new DeleteChatCommand(id), cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/join")]
        public async Task<IActionResult> JoinChat(Guid id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new JoinChatCommand(id, _userContext.UserId), cancellationToken);
            return Ok();
        }
        [HttpPost("{id}/leave")]
        public async Task<IActionResult> LeaveChat(Guid id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new LeaveChatCommand(id, _userContext.UserId), cancellationToken);
            return Ok();
        }
        [HttpGet("{id}/members")]
        public async Task<ActionResult<IPagedResult<ChatMemberDto>>> GetMembers(Guid id, int offset = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            var members = await _chatQueries.GetChatMembers(id, offset, limit, cancellationToken);
            return Ok(members);
        }
        [HttpGet("read-statuses")]
        public async Task<ActionResult<IEnumerable<ChatReadStatusDto>>> GetReadStatuses(CancellationToken cancellationToken = default)
        {
            var statuses = await _chatQueries.GetChatReadStatusesAsync(_userContext.UserId, cancellationToken);
            return Ok(statuses);
        }
        [HttpGet("{id}/read-status")]
        public async Task<ActionResult<ChatReadStatusDto>> GetReadStatus(Guid id, CancellationToken cancellationToken = default)
        {
            var status = await _chatQueries.GetChatReadStatusAsync(id, _userContext.UserId, cancellationToken);
            return Ok(status);
        }
        [HttpPost("{id}/read-status")]
        public async Task<IActionResult> UpdateReadStatus(Guid id, [FromBody] UpdateChatReadStatusRequest request, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UpdateChatReadStatusCommand(id, _userContext.UserId, request.MessageId), cancellationToken);
            return Ok();
        }
    }
}

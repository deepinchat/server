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

        [HttpGet("direct/{id}")]
        public async Task<ActionResult<DirectChatDto>> GetDirectChat(Guid id, CancellationToken cancellationToken = default)
        {
            var chat = await _mediator.Send(new GetDirectChatCommand(id), cancellationToken);
            if (chat == null)
                return NotFound();
            return Ok(chat);
        }
        [HttpGet("direct")]
        public async Task<ActionResult<IEnumerable<DirectChatDto>>> GetDirectChats(CancellationToken cancellationToken = default)
        {
            var chats = await _mediator.Send(new GetDirectChatsCommand(_userContext.UserId), cancellationToken);
            return Ok(chats);
        }
        [HttpPost("direct")]
        public async Task<ActionResult<DirectChatDto>> CreateDirectChat([FromBody] CreateDirectChatRequest request, CancellationToken cancellationToken = default)
        {
            var chat = await _mediator.Send(new CreateDirectChatCommand(request.UserId), cancellationToken);
            return CreatedAtAction(nameof(GetDirectChat), new { id = chat.Id }, chat);
        }
        [HttpGet("group/{id}")]
        public async Task<ActionResult<GroupChatDto>> GetGroupChat(Guid id, CancellationToken cancellationToken = default)
        {
            var chat = await _mediator.Send(new GetGroupChatCommand(id), cancellationToken);
            if (chat == null)
                return NotFound();
            return Ok(chat);
        }
        [HttpPost("group")]
        public async Task<ActionResult<GroupChatDto>> CreateGroupChat([FromBody] CreateGroupChatRequest request, CancellationToken cancellationToken = default)
        {
            var chat = await _mediator.Send(new CreateGroupChatCommand(
                Name: request.Name,
                UserName: request.UserName,
                Description: request.Description,
                AvatarFileId: request.AvatarFileId,
                IsPublic: request.IsPublic
            ), cancellationToken);
            return CreatedAtAction(nameof(GetGroupChat), new { id = chat.Id }, chat);
        }
        [HttpGet("group")]
        public async Task<ActionResult<IEnumerable<GroupChatDto>>> GetGroupChats(CancellationToken cancellationToken = default)
        {
            var chats = await _mediator.Send(new GetGroupChatsCommand(_userContext.UserId), cancellationToken);
            return Ok(chats);
        }

        [HttpPut("group/{id}")]
        public async Task<ActionResult<GroupChatDto>> UpdateGroupChat(Guid id, [FromBody] UpdateChatRequest request, CancellationToken cancellationToken = default)
        {
            var chat = await _mediator.Send(new UpdateGroupChatCommand(
                Id: id,
                Name: request.Name,
                UserName: request.UserName,
                Description: request.Description,
                AvatarFileId: request.AvatarFileId,
                IsPublic: request.IsPublic
            ), cancellationToken);
            return Ok(chat);
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
        [HttpGet("{id}/members/{memberId}")]
        public async Task<ActionResult<IPagedResult<ChatMemberDto>>> GetMember(Guid id, Guid memberId, CancellationToken cancellationToken = default)
        {
            var member = await _chatQueries.GetChatMember(id, memberId, cancellationToken);
            if (member == null)
                return NotFound();
            return Ok(member);
        }
        [HttpGet("{id}/members")]
        public async Task<ActionResult<IPagedResult<ChatMemberDto>>> GetMembers(Guid id, int offset = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            var members = await _chatQueries.GetChatMembers(id, offset, limit, cancellationToken);
            return Ok(members);
        }
        [HttpGet("{id}/unread-count")]
        public async Task<ActionResult<ChatUnreadCount>> GetUnreadCount(Guid id, CancellationToken cancellationToken = default)
        {
            var count = await _chatQueries.GetChatUnreadCountAsync(id, _userContext.UserId, cancellationToken);
            if (count == null)
                return NotFound();
            return Ok(count);
        }
        [HttpGet("unread-counts")]
        public async Task<ActionResult<IEnumerable<ChatUnreadCount>>> GetUnreadCounts(CancellationToken cancellationToken = default)
        {
            var counts = await _chatQueries.GetChatUnreadCountsAsync(_userContext.UserId, cancellationToken);
            return Ok(counts);
        }
    }
}

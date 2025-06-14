using Deepin.Domain.ChatAggregate;

namespace Deepin.Application.DTOs.Chats;

public class ChatMemberDto
{
    public Guid UserId { get; set; }
    public string? DisplayName { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ChatMemberRole Role { get; set; }
}

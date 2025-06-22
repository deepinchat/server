using Deepin.Domain.ChatAggregate;

namespace Deepin.Application.DTOs.Chats;

public class ChatMemberDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? DisplayName { get; set; }
    public bool IsMuted { get; set; }
    public bool IsBanned { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ChatMemberRole Role { get; set; }
}


using Deepin.Domain.ChatAggregate;

namespace Deepin.Application.DTOs.Chats;

public class ChatDto
{
    public Guid Id { get; set; }
    public ChatType Type { get; set; }
    public required string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public ChatGroupInfoDto? GroupInfo { get; set; }
    public ChatReadStatusDto? ReadStatus { get; set; }
}

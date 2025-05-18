using Deepin.Domain.ChatAggregate;

namespace Deepin.Application.DTOs.Chats;

public class ChatDto
{
    public Guid Id { get; set; }
    public ChatType Type { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ChatGroupInfoDto? GroupInfo { get; set; }
    public ChatReadStatusDto? ReadStatus { get; set; }
}

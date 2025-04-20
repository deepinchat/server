using Deepin.Domain.ChatAggregate;

namespace Deepin.Application.Models.Chats;

public class ChatDto
{
    public Guid Id { get; set; }
    public ChatType Type { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public ChatInfo? GroupInfo { get; set; }
}
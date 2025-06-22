namespace Deepin.Application.DTOs.Chats;

public class DirectChatDto
{
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<ChatMemberDto> Members { get; set; } = [];
}

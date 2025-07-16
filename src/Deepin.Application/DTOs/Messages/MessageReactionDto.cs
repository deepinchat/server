namespace Deepin.Application.DTOs.Messages;

public class MessageReactionDto
{
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public string Emoji { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}

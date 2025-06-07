namespace Deepin.SDK.Models;

/// <summary>
/// Represents a message in the system
/// </summary>
public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public MessageType Type { get; set; }
    public User? Sender { get; set; }
    public Chat? Chat { get; set; }
    public List<FileAttachment>? Attachments { get; set; }
}

/// <summary>
/// Represents the type of message
/// </summary>
public enum MessageType
{
    Text = 0,
    File = 1,
    Image = 2,
    System = 3
}

/// <summary>
/// Request model for sending a message
/// </summary>
public class SendMessageRequest
{
    public string Content { get; set; } = string.Empty;
    public int ChatId { get; set; }
    public MessageType Type { get; set; } = MessageType.Text;
    public List<int>? FileIds { get; set; }
}

/// <summary>
/// Request model for getting messages
/// </summary>
public class GetMessagesRequest
{
    public List<int> MessageIds { get; set; } = new();
}

/// <summary>
/// Request model for searching messages
/// </summary>
public class SearchMessagesRequest
{
    public string Query { get; set; } = string.Empty;
    public int? ChatId { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 20;
}

/// <summary>
/// Request model for getting last messages
/// </summary>
public class GetLastMessagesRequest
{
    public List<int> ChatIds { get; set; } = new();
}

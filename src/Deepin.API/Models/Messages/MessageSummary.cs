using Deepin.API.Models.Users;
using Deepin.Application.DTOs.Messages;

namespace Deepin.API.Models.Messages;

public class MessageSummary
{
    public required MessageDto Message { get; set; }
    public UserProfile? Sender { get; set; }
    public IEnumerable<MessageReactionDto>? Reactions { get; set; }
}

using Deepin.Application.DTOs;

namespace Deepin.API.Models.Messages;

public class SearchMessageRequest : PagedQuery
{
    public DateTimeOffset? ReadAt { get; set; }
    public Guid? ChatId { get; set; }
    public Guid? UserId { get; set; }
}

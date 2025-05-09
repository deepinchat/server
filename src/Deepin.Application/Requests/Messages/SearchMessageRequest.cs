using Deepin.Application.DTOs;

namespace Deepin.Application.Requests.Messages;

public class SearchMessageRequest : PagedQuery
{
    public Guid? ChatId { get; set; }
    public Guid? SenderId { get; set; }
}

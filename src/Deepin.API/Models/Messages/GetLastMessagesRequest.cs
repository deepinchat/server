namespace Deepin.API.Models.Messages;

public class GetLastMessagesRequest
{
    public List<Guid> ChatIds { get; set; } = [];
}

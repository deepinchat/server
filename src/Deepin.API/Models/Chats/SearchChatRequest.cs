using Deepin.Domain.ChatAggregate;

namespace Deepin.API.Models.Chats;

public class SearchChatRequest
{
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;
    public string? Search { get; set; }
    public ChatType? Type { get; set; }
}

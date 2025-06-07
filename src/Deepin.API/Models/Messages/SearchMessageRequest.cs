using System;

namespace Deepin.API.Models.Messages;

public class SearchMessageRequest
{
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;
    public string? Search { get; set; }
    public Guid? ChatId { get; set; }
    public Guid? UserId { get; set; }
}

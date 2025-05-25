namespace Deepin.API.Models.Users;

public class SearchUserRequest
{
    public int Limit { get; set; } = 20;
    public int Offset { get; set; } = 0;
    public string? Search { get; set; }
}

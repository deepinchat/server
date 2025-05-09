namespace Deepin.Application.DTOs;

public class PagedQuery
{
    public int Offset { get; set; }

    public int Limit { get; set; } = 10;

    public string? OrderBy { get; set; }

    public string? OrderByDesc { get; set; }

    public string? Filter { get; set; }

    public string? Search { get; set; }
}

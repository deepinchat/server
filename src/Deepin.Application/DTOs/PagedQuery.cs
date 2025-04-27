namespace Deepin.Application.DTOs;

public class PagedQuery
{
    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? OrderBy { get; set; }

    public string? OrderByDesc { get; set; }

    public string? Filter { get; set; }

    public string? Search { get; set; }
}

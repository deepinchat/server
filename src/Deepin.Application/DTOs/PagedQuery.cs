namespace Deepin.Application.DTOs;

public class PagedQuery
{
    public int Offset { get; set; }

    public int Limit { get; set; } = 10;

    public SortDirection? SortBy { get; set; }

    public string? SortKey { get; set; }

    public string? Filter { get; set; }

    public string? Search { get; set; }
}

public enum SortDirection
{
    Ascending,
    Descending
}
public interface IPagedResult<T>
{
    IEnumerable<T> Items { get; set; }
    int Offset { get; set; }
    int Limit { get; set; }
    int TotalCount { get; set; }
    bool HasMore { get; set; }
}
public class PagedResult<T> : IPagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int Offset { get; set; }
    public int Limit { get; set; }
    public int TotalCount { get; set; }
    public bool HasMore { get; set; }
    public PagedResult() { }
    public PagedResult(IEnumerable<T> items, int offset, int limit, int totalCount)
    {
        Items = items;
        Offset = offset;
        Limit = limit;
        TotalCount = totalCount;
        HasMore = (offset + limit) < totalCount;
    }
    public PagedResult(IQueryable<T> source, int offset, int limit)
    {
        int totalCount = source.Count();
        TotalCount = totalCount;
        Limit = limit;
        Offset = offset;
        HasMore = (offset + limit) < totalCount;
        Items = source.Skip(offset).Take(limit);
    }
}
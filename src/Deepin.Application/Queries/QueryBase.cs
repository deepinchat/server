namespace Deepin.Application.Queries;

public abstract class QueryBase(string schema)
{
    protected string BuildSqlWithSchema(string sql)
    {
        return $"set search_path to {schema}; {sql}";
    }
}

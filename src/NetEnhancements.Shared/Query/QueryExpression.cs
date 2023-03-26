namespace NetEnhancements.Shared.Query
{
    public class QueryExpression
    {
        public string Query { get; }

        public QueryExpression(string query)
        {
            Query = query;
        }

        public static implicit operator QueryExpression(string query)
        {
            return new QueryExpression(query);
        }
    }
}

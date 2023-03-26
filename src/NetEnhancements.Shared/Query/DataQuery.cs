namespace NetEnhancements.Shared.Query
{
    /// <summary>
    /// Allows for passing query strings and sort clauses from controllers to managers.
    ///
    /// TODO: nested queries, range queries, this is a POC.
    /// </summary>
    public class DataQuery : PagedQuery
    {
        private readonly List<QueryExpression> _queryExpressions = new(0);
        private readonly List<OrderExpression> _orderExpressions = new(0);

        public IReadOnlyCollection<QueryExpression> QueryExpressions => _queryExpressions.AsReadOnly();
        public IReadOnlyCollection<OrderExpression> OrderExpressions => _orderExpressions.AsReadOnly();

        public static DataQuery Query(string? query)
        {
            var q = new DataQuery();
            
            if (!string.IsNullOrWhiteSpace(query))
            {
                q._queryExpressions.Add(new QueryExpression(query));
            }
            
            return q;
        }

        public DataQuery AddQuery(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                _queryExpressions.Add(new QueryExpression(query));
            }

            return this;
        }

        public static DataQuery Order(string? order, OrderDirection direction = OrderDirection.Ascending)
        {
            var q = new DataQuery();
            
            if (!string.IsNullOrWhiteSpace(order))
            {
                q._orderExpressions.Add(new OrderExpression(order, direction));
            }
            
            return q;
        }

        public DataQuery AddOrder(string? order, OrderDirection direction = OrderDirection.Ascending)
        {
            if (string.IsNullOrWhiteSpace(order))
            {
                return this;
            }

            if (order.EndsWith("_desc", StringComparison.InvariantCultureIgnoreCase))
            {
                order = order[..^5];
                direction = OrderDirection.Descending;
            }

            _orderExpressions.Add(new OrderExpression(order, direction));

            return this;
        }

        public DataQuery SetPageNumber(int page)
        {
            PageNumber = page;

            return this;
        }

        public DataQuery SetPerPage(int perPage)
        {
            PerPage = perPage;

            return this;
        }
    }
}
